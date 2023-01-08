using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public interface IWzSprite
    {
        Dictionary<string, WzFrame> Frames { get; }
    }
}
