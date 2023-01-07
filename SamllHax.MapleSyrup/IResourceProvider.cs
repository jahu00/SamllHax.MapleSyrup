using SamllHax.MapleSyrup.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public interface IResourceProvider
    {
        WzMap GetMap(int id);

        WzTileSet GetTileSet(string name);

        WzObjectGroup GetObjectGroup(string name);

        public Stream GetTileImage(string tileSetName, string tileName, int variant);

        public Stream GetObjectImage(string objectGroupName, string objectName, string subSetName, string partId, string frameId);
    }
}
