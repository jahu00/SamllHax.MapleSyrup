using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class UpdateEvents
    {
        public UpdateEvents(float delta, KeyboardState keyboardState)
        {
            Delta = delta;
            KeyboardState = keyboardState;
        }

        public float Delta { get; }

        public KeyboardState KeyboardState { get; }
    }
}
