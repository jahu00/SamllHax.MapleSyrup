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
using SamllHax.MapleSyrup.Interfaces.Providers;
using SamllHax.MapleSyrup.Interfaces.Data;

namespace SamllHax.MapleSyrup
{
    public class ResourceManager
    {
        private readonly IResourceProvider _resourceProvider;
        private readonly IConfiguration _configuration;

        //public Dictionary<string, WzTileSet> TileSetCatche { get; } = new Dictionary<string, WzTileSet>();
        public Dictionary<string, IEntity> EntityCache { get; } = new Dictionary<string, IEntity>();
        public Dictionary<string, SKBitmap> ImageCatche { get; } = new Dictionary<string, SKBitmap>();

        public ResourceManager(IConfiguration configuration, IResourceProvider resourceProvider)
        {
            //_configuration = configuration.GetSection("DumperResourceManager");
            _resourceProvider = resourceProvider;
        }

        public IMap GetMap(int id)
        {
            return GetEntityFromCache($"Map-{id}", () => _resourceProvider.GetMap(id));
        }

        public IEntityDirectory<IFrame> GetTileSet(string name)
        {
            return GetEntityFromCache($"TileSet-{name}", () => _resourceProvider.GetTileSet(name));
        }

        public IEntityDirectory<IAnimation> GetObjectDirectory(string name)
        {
            return GetEntityFromCache($"ObjectGroup-{name}", () => _resourceProvider.GetObjectDirectory(name));
        }

        public T GetEntityFromCache<T>(string key, Func<T> fallback) where T : IEntity
        {
            IEntity entity;
            if (!EntityCache.TryGetValue(key, out entity))
            {
                entity = fallback();
                EntityCache.Add(key, entity);
            }
            return (T)entity;
        }

        public SKBitmap GetTileImage(string tileSetName, string[] path)
        {
            var key = $"TileSet-{tileSetName}-{string.Join("-", path)}";
            return GetImageFromCache(key, () => _resourceProvider.GetTileImage(tileSetName, path));
        }

        public SKBitmap GetObjectImage(string objectDirectoryName, string[] path, string frameId)
        {
            var key = $"Obj-{objectDirectoryName}-{string.Join("-", path)}-{frameId}";
            return GetImageFromCache(key, () => _resourceProvider.GetObjectImage(objectDirectoryName, path, frameId));
        }

        public SKBitmap GetMobImage(string mobId, string animation, string frameId)
        {
            var key = $"Mob-{mobId}-{animation}-{frameId}";
            return GetImageFromCache(key, () => _resourceProvider.GetMobImage(mobId, animation, frameId));
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
