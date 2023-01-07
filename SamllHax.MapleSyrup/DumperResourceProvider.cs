using Microsoft.Extensions.Configuration;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SamllHax.MapleSyrup.Data;
using SamllHax.MapleSyrup.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SamllHax.MapleSyrup
{
    public class DumperResourceProvider : IResourceProvider
    {
        private readonly string _path;
        private readonly IConfiguration _configuration;
        public DumperResourceProvider(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("DumperResourceProvider");
            _path = _configuration.GetValue<string>("Path");
        }

        public WzMap GetMap(int id)
        {
            var strId = id.ToString().PadLeft(9, '0');
            var areaSuffix = id / 100000000;
            var relativePath = $"Map.wz/Map/Map{areaSuffix}/{strId}.img";
            return GetEntityFromXml(relativePath, (node) => new WzMap(node));
        }

        public WzObjectGroup GetObjectGroup(string name)
        {
            var relativeFilePath = $"Map.wz/Obj/{name}.img";
            return GetEntityFromXml(relativeFilePath, (node) => new WzObjectGroup(node));
        }

        public WzTileSet GetTileSet(string name)
        {
            var relativeFilePath = $"Map.wz/Tile/{name}.img";
            return GetEntityFromXml(relativeFilePath, (node) => new WzTileSet(node));
        }

        public T GetEntityFromXml<T>(string relativePath, Func<WzDirectory, T> getEntity) where T : WzEntity
        {
            var filePath = Path.Combine(_path, relativePath + ".xml");
            var xml = XElement.Load(filePath);
            var node = (WzDirectory)ParseNode(xml);
            var entity = getEntity(node);
            return entity;
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

        public Stream GetTileImage(string tileSetName, string tileName, int variant)
        {
            var relativeFilePath = $"Map.wz/Tile/{tileSetName}.img/{tileName}/{variant}";
            return GetImage(relativeFilePath);
        }

        public Stream GetObjectImage(string objectGroupName, string objectName, string subSetName, string partId, string frameId)
        {
            var relativeFilePath = $"Map.wz/Obj/{objectGroupName}.img/{objectName}/{subSetName}/{partId}/{frameId}";
            return GetImage(relativeFilePath);
        }

        public Stream GetImage(string relativeFilePath)
        {

            var filePath = Path.Combine(_path, relativeFilePath + ".png");
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return stream;
        }

        
    }
}
