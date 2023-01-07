using SamllHax.MapleSyrup.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using SamllHax.MapleSyrup.Extensions;
using SkiaSharp;
using Microsoft.Extensions.Configuration;

namespace SamllHax.MapleSyrup
{
    public class ResourceManager
    {
        private readonly string _path;
        private readonly IConfiguration _configuration;

        //public Dictionary<string, WzTileSet> TileSetCatche { get; } = new Dictionary<string, WzTileSet>();
        public Dictionary<string, WzEntity> EntityCache { get; } = new Dictionary<string, WzEntity>();
        public Dictionary<string, SKBitmap> ImageCatche { get; } = new Dictionary<string, SKBitmap>();

        public ResourceManager(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("DumperResourceManager");
            _path = _configuration.GetValue<string>("Path");
        }

        public WzMap GetMap(int id)
        {
            var strId = id.ToString().PadLeft(9, '0');
            var areaSuffix = id / 100000000;
            var filePath = Path.Combine(_path, "Map.wz", "Map", $"Map{areaSuffix}", $"{strId}.img.xml");
            var xml = XElement.Load(filePath);
            var node = (WzDirectory)ParseNode(xml);
            var map = new WzMap(node);
            return map;
        }

        public WzTileSet GetTileSet(string name)
        {
            var relativeFilePath = Path.Combine("Map.wz", "Tile", $"{name}.img.xml");
            return GetEntityFromCache(relativeFilePath, (node) =>
            {
                return new WzTileSet(node);
            });
        }

        public WzObjectGroup GetObjectGroup(string name)
        {
            var relativeFilePath = Path.Combine("Map.wz", "Obj", $"{name}.img.xml");
            return GetEntityFromCache(relativeFilePath, (node) =>
            {
                return new WzObjectGroup(node);
            });
        }

        public T GetEntityFromCache<T>(string key, Func<WzDirectory,T> getEntity) where T : WzEntity
        {
            WzEntity entity;
            if (!EntityCache.TryGetValue(key, out entity))
            {
                var filePath = Path.Combine(_path, key);
                var xml = XElement.Load(filePath);
                var node = (WzDirectory)ParseNode(xml);
                entity = getEntity(node);
                EntityCache.Add(key, entity);
            }
            return (T)entity;
        }

        public SKBitmap GetTileImage(string tileSetName, string tileName, int variant)
        {
            var relativeFilePath = Path.Combine("Map.wz", "Tile", $"{tileSetName}.img", tileName, $"{variant}.png");
            return GetImage(relativeFilePath);
        }

        public SKBitmap GetObjectImage(string objectGroupName, string objectName, string subSetName, string partId, string frameId)
        {
            var relativeFilePath = Path.Combine("Map.wz", "Obj", $"{objectGroupName}.img", objectName, subSetName, partId, $"{frameId}.png");
            return GetImage(relativeFilePath);
        }

        public SKBitmap GetImage(string relativeFilePath)
        {
            SKBitmap bitmap;
            if (!ImageCatche.TryGetValue(relativeFilePath, out bitmap))
            {
                var filePath = Path.Combine(_path, relativeFilePath);
                bitmap = SKBitmap.Decode(filePath);
                ImageCatche.Add(relativeFilePath, bitmap);
            }
            return bitmap;
        }

        public WzNode ParseNode(XElement xml)
        {
            WzNode node;
            var xmlName = xml.Name.ToString();
            switch (xmlName)
            {
                case "imgdir":
                    node = new WzDirectory();
                    break;
                case "extended":
                    node = new WzDirectory();
                    break;
                case "int":
                    node = new WzIntValue() { Value = xml.Attribute("value").ValueAsInt() };
                    break;
                case "float":
                    node = new WzFloatValue() { Value = xml.Attribute("value").ValueAsFloat() };
                    break;
                case "string":
                    node = new WzStringValue() { Value = xml.Attribute("value").Value };
                    break;
                case "canvas":
                    node = new WzCanvas() { Width = xml.Attribute("width").ValueAsInt(), Height = xml.Attribute("height").ValueAsInt() };
                    break;
                case "vector":
                    node = new WzVector() { X = xml.Attribute("x").ValueAsInt(), Y = xml.Attribute("y").ValueAsInt() };
                    break;
                case "uol":
                    node = new WzRepeat() { Value = xml.Attribute("value").Value };
                    break;
                default:
                    //node = new WzNode();
                    throw new Exception($"Unupported xml node type {xmlName}");
                    break;
            }
            node.Name = xml.Attribute("name").Value;
            node.Xml = xml;
            var nodeAsDirectory = node as WzDirectory;
            var childrenXml = xml.Nodes().ToList();
            if (childrenXml.Count() > 0)
            {
                if (nodeAsDirectory == null)
                {
                    throw new Exception($"Node {node.Name} of type {node.GetType().Name} is not a directory");
                }
                nodeAsDirectory.Children = childrenXml.Select(childXml => ParseNode((XElement)childXml)).ToList();
            }
            return node;
        }
    }
}
