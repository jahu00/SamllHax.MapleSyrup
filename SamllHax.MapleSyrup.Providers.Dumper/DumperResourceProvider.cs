using Microsoft.Extensions.Configuration;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Providers;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Extensions;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SamllHax.MapleSyrup.Providers.Dumper
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

        public IMap GetMap(int id)
        {
            var strId = id.ToString().PadLeft(9, '0');
            var areaSuffix = id / 100000000;
            var relativePath = $"Map.wz/Map/Map{areaSuffix}/{strId}.img";
            return GetEntityFromXml(relativePath, (node) => new WzMap(node));
        }

        public IEntityDirectory<IAnimation> GetObjectDirectory(string name)
        {
            var relativeFilePath = $"Map.wz/Obj/{name}.img";
            return GetEntityFromXml(relativeFilePath, (node) => new WzObjectDirectory(node));
        }

        public IEntityDirectory<IFrame> GetTileSet(string name)
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
                    node = new WzExtended();
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
#if DEBUG
            node.Xml = xml;
#endif
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

        public Stream GetTileImage(string tileSetName, string[] path)
        {
            var relativeFilePath = $"Map.wz/Tile/{tileSetName}.img/{string.Join("/", path)}";
            return GetImage(relativeFilePath);
        }

        public Stream GetObjectImage(string objectDirectory, string[] path, string frameId)
        {
            var relativeFilePath = $"Map.wz/Obj/{objectDirectory}.img/{string.Join("/", path)}/{frameId}";
            return GetImage(relativeFilePath);
        }

        public Stream GetMobImage(string mobId, string animation, string frameId)
        {
            var relativeFilePath = $"Mob.wz/{mobId}.img/{animation}/{frameId}";
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
