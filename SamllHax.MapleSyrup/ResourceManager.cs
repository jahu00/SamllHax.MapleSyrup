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
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;

namespace SamllHax.MapleSyrup
{
    public class ResourceManager
    {
        private readonly IResourceProvider _resourceProvider;
        private readonly GrContextManager _contextManager;
        private readonly IConfiguration _configuration;

        //public Dictionary<string, WzTileSet> TileSetCatche { get; } = new Dictionary<string, WzTileSet>();
        public Dictionary<string, CachedResourceWrapper> ResourceCache { get; } = new Dictionary<string, CachedResourceWrapper>();
        public Dictionary<object, List<CachedResourceWrapper>> OwnerIndex { get; } = new Dictionary<object, List<CachedResourceWrapper>>();

        public ResourceManager(IConfiguration configuration, IResourceProvider resourceProvider, GrContextManager contextManager)
        {
            //_configuration = configuration.GetSection("DumperResourceManager");
            _resourceProvider = resourceProvider;
            _contextManager = contextManager;
        }

        public IMap GetMap(object owner, int id)
        {
            return GetFromCache($"Map-{id}", owner, () => _resourceProvider.GetMap(id));
        }

        public IEntityDirectory<IFrame> GetTileSet(object owner, string name)
        {
            return GetFromCache($"TileSet-{name}", owner, () => _resourceProvider.GetTileSet(name));
        }

        public IEntityDirectory<IAnimation> GetObjectDirectory(object owner, string name)
        {
            return GetFromCache($"ObjectGroup-{name}", owner, () => _resourceProvider.GetObjectDirectory(name));
        }

        internal IMapHelpers GetMapHelpers(object owner)
        {
            return GetFromCache($"MapHelpers", owner, () => _resourceProvider.GetMapHelpers());
        }

        public T GetFromCache<T>(string key, object owner, Func<T> fallback)
        {
            CachedResourceWrapper cachedResourceWrapper;
            T resource;
            if (!ResourceCache.TryGetValue(key, out cachedResourceWrapper))
            {
                resource = fallback();
                cachedResourceWrapper = new CachedResourceWrapper(resource, owner);
                ResourceCache.Add(key, cachedResourceWrapper);
            }
            else if (!cachedResourceWrapper.Owners.Contains(owner))
            {
                cachedResourceWrapper.Owners.Add(owner);
            }
            if (OwnerIndex.TryGetValue(owner, out var ownedResources) && !ownedResources.Contains(cachedResourceWrapper))
            {
                ownedResources.Add(cachedResourceWrapper);
            }
            return cachedResourceWrapper.Cast<T>();
        }

        public void AbandonResources(object owner)
        {
            if (!OwnerIndex.TryGetValue(owner, out var ownedResources))
            {
                return;
            }
            ownedResources.ForEach(x => x.Owners.Remove(owner));
            OwnerIndex.Remove(owner);
        }

        public void FreeResources()
        {
            var abandonedReourcePairs = ResourceCache.Where(x => x.Value.IsAbandoned).Select(x => new { x.Key, x.Value }).ToList();
            abandonedReourcePairs.ForEach
            (
                pair =>
                {
                    pair.Value.Dispose();
                    ResourceCache.Remove(pair.Key);
                }
            );
        }

        public SKImage GetTileImage(object owner, string tileSetName, string[] path)
        {
            var key = $"TileSet-{tileSetName}-{string.Join("-", path)}";
            return GetFromCache(key, owner, () => _resourceProvider.GetTileImage(tileSetName, path).ToImage());
        }

        public SKImage GetObjectImage(object owner, string objectDirectoryName, string[] path, string frameId)
        {
            var key = $"Obj-{objectDirectoryName}-{string.Join("-", path)}-{frameId}";
            return GetFromCache(key, owner, () => _resourceProvider.GetObjectImage(objectDirectoryName, path, frameId).ToImage());
        }

        public SKImage GetMobImage(object owner, string mobId, string animation, string frameId)
        {
            var key = $"Mob-{mobId}-{animation}-{frameId}";
            return GetFromCache(key, owner, () => _resourceProvider.GetMobImage(mobId, animation, frameId).ToImage());
        }

        public SKImage GetImage(object owner, string file, string[] path, string frameId)
        {
            var key = $"{file}-{string.Join("-", path)}-{frameId}";
            return GetFromCache(key, owner, () => _resourceProvider.GetImage(file, path, frameId).ToImage());
        }

        public Dictionary<string, SKImage> GetImages(object owner, DataFiles dataFile, IEnumerable<string> path, IEnumerable<string> frameIds)
        {
            return GetImages(owner, dataFile.ToString(), path, frameIds);
        }

        public Dictionary<string, SKImage> GetImages(object owner, string file, IEnumerable<string> path, IEnumerable<string> frameIds)
        {
            var key = $"{file}-{string.Join("-", path)}-frames";
            return GetFromCache(key, owner, () => frameIds.Select(frameId => new { FrameId = frameId, Bitmap = _resourceProvider.GetImage(file, path, frameId).ToImage() }).ToDictionary(x => x.FrameId, x => x.Bitmap));
        }

        public Dictionary<string, SKImage> GetImages(object owner, DataFiles dataFile, IEnumerable<string> path, IDictionary<string,string> frameIdMappings)
        {
            return GetImages(owner, dataFile.ToString(), path, frameIdMappings);
        }

        public Dictionary<string, SKImage> GetImages(object owner, string file, IEnumerable<string> path, IDictionary<string, string> frameIdMappings)
        {
            var key = $"{file}-{string.Join("-", path)}-frames";
            return GetFromCache(key, owner, () => frameIdMappings.Select(frameIdMapping => new { FrameId = frameIdMapping.Key, Bitmap = _resourceProvider.GetImage(file, path, frameIdMapping.Value).ToImage() }).ToDictionary(x => x.FrameId, x => x.Bitmap));
        }
    }
}
