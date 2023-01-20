using SamllHax.MapleSyrup.Draw;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class PlayerInstance : DrawableBase, IDrawable, IUpdatable
    {
        private CommonData _commonData;

        public double SpeedX { get; set; }
        public double SpeedY { get; set; }

        public IDrawable Sprite { get; set; }
        public MapInstance MapInstance { get; set; }

        public PlayerInstance(CommonData commonData)
        {
            _commonData = commonData;
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            Sprite.Draw(canvas, this.GetTransformMatrix(matrix));
        }

        public void Update(double delta)
        {
            if (SpeedY < _commonData.Physics.FallSpeed)
            {
                SpeedY += (float)(delta * _commonData.Physics.GravityAcc);
            }

            var mapBoundingBox = MapInstance.GetBoundingBox();
            X += (float)(delta * SpeedX);
            Y += (float)(delta * SpeedY);
            
            if (Y > mapBoundingBox.Bottom)
            {
                Y = mapBoundingBox.Bottom;
            } else if (Y < mapBoundingBox.Top)
            {
                Y = mapBoundingBox.Top;
            }

            if (X < mapBoundingBox.Left)
            {
                X = mapBoundingBox.Left;
            } else if (X > mapBoundingBox.Right)
            {
                X = mapBoundingBox.Right;
            }
        }
    }
}
