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
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SamllHax.MapleSyrup
{
    public class DumperResourceManager
    {
        private readonly IResourceProvider _resourceProvider;
        private readonly IConfiguration _configuration;

        //public Dictionary<string, WzTileSet> TileSetCatche { get; } = new Dictionary<string, WzTileSet>();
        public Dictionary<string, WzEntity> EntityCache { get; } = new Dictionary<string, WzEntity>();
        public Dictionary<string, SKBitmap> ImageCatche { get; } = new Dictionary<string, SKBitmap>();

        public DumperResourceManager(IConfiguration configuration, IResourceProvider resourceProvider)
        {
            //_configuration = configuration.GetSection("DumperResourceManager");
            _resourceProvider = resourceProvider;
        }

        public WzMap GetMap(int id)
        {
            return GetEntityFromCache($"Map-{id}", () => _resourceProvider.GetMap(id));
        }

        public WzTileSet GetTileSet(string name)
        {
            return GetEntityFromCache($"TileSet-{name}", () => _resourceProvider.GetTileSet(name));
        }

        public WzObjectGroup GetObjectGroup(string name)
        {
            return GetEntityFromCache($"ObjectGroup-{name}", () => _resourceProvider.GetObjectGroup(name));
        }

        public T GetEntityFromCache<T>(string key, Func<T> fallback) where T : WzEntity
        {
            WzEntity entity;
            if (!EntityCache.TryGetValue(key, out entity))
            {
                entity = fallback();
                EntityCache.Add(key, entity);
            }
            return (T)entity;
        }

        public SKBitmap GetTileImage(string tileSetName, string tileName, int variant)
        {
            var key = $"TileSet-{tileSetName}-{tileName}-{variant}";
            return GetImageFromCache(key, () => _resourceProvider.GetTileImage(tileSetName, tileName, variant));
        }

        public SKBitmap GetObjectImage(string objectGroupName, string objectName, string subSetName, string partId, string frameId)
        {
            var key = $"Obj-{objectGroupName}-{ objectName}-{ subSetName}-{ partId}-{frameId}";
            return GetImageFromCache(key, () => _resourceProvider.GetObjectImage(objectGroupName, objectName, subSetName, partId, frameId));
        }

        public SKBitmap GetImageFromCache(string key, Func<Stream> fallback)
        {
            SKBitmap bitmap;
            if (!ImageCatche.TryGetValue(key, out bitmap))
            {
                var stream = fallback();
                bitmap = SKBitmap.Decode(stream);
                ImageCatche.Add(key, bitmap);
            }
            return bitmap;
        }

        
    }
}
