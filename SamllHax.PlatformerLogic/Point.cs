using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public class Point: IPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        
        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
