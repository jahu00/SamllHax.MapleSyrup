using SamllHax.MapleSyrup.Interfaces;
using SamllHax.MapleSyrup.Interfaces.Data;
using System.IO;

namespace SamllHax.MapleSyrup.Interfaces.Providers
{
    public interface IResourceProvider
    {
        IMap GetMap(int id);

        IEntityDirectory<IFrame> GetTileSet(string name);

        IEntityDirectory<IAnimation> GetObjectDirectory(string name);

        Stream GetTileImage(string tileSetName, string[] path);

        Stream GetObjectImage(string objectDirectoryName, string[] path, string frameId);
    }
}
