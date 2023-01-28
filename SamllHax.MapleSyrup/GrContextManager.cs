using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    /// <summary>
    /// Class for managing GRContext. It appeared it might be required for passing to SKImage to ensure GPU backed bitmaps.
    /// It's slightly better to force SkImages to be texture backed from the start, as automatic movement to textures
    /// happens when they are first drawn, causing slight fps hiccup (as opposite to longer loading time).
    /// </summary>
    public class GrContextManager: IDisposable
    {
        public GRGlInterface GRGlInterface { get; private set;  }
        public GRContext GRContext { get; private set;  }

        public GrContextManager()
        {
        }

        public void Init()
        {
            GRGlInterface = GRGlInterface.Create();
            GRContext = GRContext.CreateGl(GRGlInterface);
        }

        public void Dispose()
        {
            GRContext?.Dispose();
            GRGlInterface?.Dispose();
        }
    }
}
