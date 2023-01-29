using SamllHax.MapleSyrup.Interfaces;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using System.Collections.Generic;
using System.IO;

namespace SamllHax.MapleSyrup.Interfaces.Providers
{
    public interface IResourceProvider
    {
        IMap GetMap(int id);

        IEntityDirectory<IFrame> GetTileSet(string name);

        IEntityDirectory<IAnimation> GetObjectDirectory(string name);

        IMapHelpers GetMapHelpers();
        IPhysics GetPhysics();

        Stream GetTileImage(string tileSetName, string[] path);

        Stream GetObjectImage(string objectDirectoryName, string[] path, string frameId);

        Stream GetMobImage(string mobId, string animation, string frameId);

        Stream GetImage(string file, IEnumerable<string> path, string frameId);
    }

    public static class IResourceManagerExtensions
    {
        public static Stream GetImage(this IResourceProvider resourceProvider, DataFiles file, string[] path, string frameId)
        {
            return resourceProvider.GetImage(file: file.ToString(), path: path, frameId: frameId);
        }
    }
}
