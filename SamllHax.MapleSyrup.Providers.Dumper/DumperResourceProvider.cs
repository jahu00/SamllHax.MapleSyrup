using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using SamllHax.MapleSyrup.Interfaces.Providers;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Extensions;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SamllHax.MapleSyrup.Providers.Dumper
{
    public class DumperResourceProvider : IResourceProvider
    {
        private readonly string _basePath;
        private readonly ILogger<DumperResourceProvider> _logger;
        private readonly IConfiguration _configuration;
        public DumperResourceProvider(IConfiguration configuration, ILogger<DumperResourceProvider> logger)
        {
            _logger = logger;
            _configuration = configuration.GetSection("DumperResourceProvider");
            _basePath = _configuration.GetValue<string>("Path");
        }

        public IMap GetMap(int id)
        {
            var strId = id.ToString().PadLeft(9, '0');
            var areaSuffix = id / 100000000;
            var relativePath = $"Map.wz/Map/Map{areaSuffix}/{strId}";
            return GetEntityFromXml(relativePath, (node) => new WzMap(node));
        }

        public IEntityDirectory<IAnimation> GetObjectDirectory(string name)
        {
            var relativeFilePath = $"Map.wz/Obj/{name}";
            return GetEntityFromXml(relativeFilePath, (node) => new WzAnimationDirectory(node));
        }

        public IEntityDirectory<IFrame> GetTileSet(string name)
        {
            var relativeFilePath = $"Map.wz/Tile/{name}";
            return GetEntityFromXml(relativeFilePath, (node) => new WzTileSet(node));
        }

        public IMapHelpers GetMapHelpers()
        {
            var relativeFilePath = $"Map.wz/MapHelper";
            return GetEntityFromXml(relativeFilePath, (node) => new WzMapHelpers(node));
        }

        public IPhysics GetPhysics()
        {
            var relativeFilePath = $"Map.wz/Physics";
            return GetEntityFromXml(relativeFilePath, (node) => new WzPhysics(node));
        }

        public T GetEntityFromXml<T>(string relativePath, Func<WzDirectory, T> getEntity) where T : WzEntity
        {
            var filePath = Path.Combine(_basePath, relativePath + ".img.xml");
            var xml = XElement.Load(filePath);
            var node = (WzDirectory)ParseNode(xml);
            var entity = getEntity(node);
            return entity;
        }

        public WzNode ParseNode(XElement xml, int watchdog = 255)
        {
            watchdog--;
            if (watchdog < 0)
            {
                throw new Exception("Watchdog triggered");
            }
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
                case "short":
                case "int":
                    node = new WzIntValue() { Value = xml.Attribute("value").ValueAsInt() };
                    break;
                case "float":
                case "double":
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
                    node = ResolveRepeatNode(xml, watchdog);
                    break;
                case "sound":
                    node = new WzSound();
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
                nodeAsDirectory.Children = childrenXml.Select(childXml => ParseNode((XElement)childXml, watchdog)).ToList();
            }
            return node;
        }

        private WzNode ResolveRepeatNode(XElement xml, int watchdog)
        {
            var repeatNode = new WzRepeat() { Value = xml.Attribute("value").Value };
            var otherXml = GetXmlByPath(xml, repeatNode.Value);
            var node = ParseNode(otherXml, watchdog);
            var canvasNode = node as WzCanvas;
            if (canvasNode == null)
            {
                throw new Exception($"Expected node to be of type WzCanvas, but got {node.GetType().Name}");
            }
            canvasNode.FramePath = repeatNode.Value;
            canvasNode.Name = repeatNode.Name;
            return node;
        }

        private XElement GetXmlByPath(XElement xml, string value)
        {
            var parts = value.Split('/');
            var result = xml.Parent;
            foreach(var part in parts)
            {
                if (part == "..")
                {
                    result = result.Parent;
                    continue;
                }
                result = result.Nodes().Cast<XElement>().First(x => x.Attribute("name").Value == part);
            }
            return result;
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

        public Stream GetImage(string file, IEnumerable<string> path, string frameId)
        {

            var relativeFilePath = Path.Combine(_basePath, $"{file}.wz/{string.Join("/", path)}/{frameId}");
            return GetImage(relativeFilePath);
        }

        public Stream GetImage(string relativeFilePath)
        {

            var filePath = Path.Combine(_basePath, relativeFilePath + ".png");
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return stream;
        }

        public string BuildPath(DataFiles dataFile, string[] path)
        {
            return BuildPath(dataFile.ToString(), path);
        }

        public string BuildPath(string file, string[] path)
        {
            return $"{file}.wz/{string.Join("/", path)}";
        }

    }
}
