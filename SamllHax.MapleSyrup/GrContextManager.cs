using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    /// <summary>
    /// Class for managing GRContext. It appeared it might be required for passing to SkImage to ensure GPU backed bitmaps, however, this appears to happen automatically (making this sowewhat redundant).
    /// TODO: Possibly move GRContext back to Game class.
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
