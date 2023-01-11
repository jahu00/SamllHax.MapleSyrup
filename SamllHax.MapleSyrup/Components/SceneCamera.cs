using SamllHax.MapleSyrup.Draw;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class SceneCamera<TScene> : DrawableBase
        where TScene: IBoundable, IDrawable
    {
        public IBoundable Container { get; set; }
        public IDrawable ObjectWithCamera { get; set; }
        public TScene Scene { get; set; }
        public void Draw(SKCanvas canvas, int x, int y)
        {
            var offsetX = X + x;
            var offsetY = Y + y;
            var sceneBoundingBox = Scene.GetBoundingBox(0, 0);
            var containerBoundingBox = Container.GetBoundingBox(0, 0);
            int sceneX;
            int sceneY;
            if (sceneBoundingBox.Width <= containerBoundingBox.Width)
            {
                sceneX = sceneBoundingBox.MidX - (containerBoundingBox.Width / 2);
            }
            else
            {
                sceneX = ObjectWithCamera.X - (containerBoundingBox.Width / 2);
                if (sceneX < sceneBoundingBox.Left)
                {
                    sceneX = sceneBoundingBox.Left;
                }
                else if (sceneX > sceneBoundingBox.Right - containerBoundingBox.Width)
                {
                    sceneX = sceneBoundingBox.Right - containerBoundingBox.Width;
                }
            }

            if (sceneBoundingBox.Height <= containerBoundingBox.Height)
            {
                sceneY = sceneBoundingBox.MidY - (containerBoundingBox.Height / 2);
            }
            else
            {
                sceneY = ObjectWithCamera.Y - (containerBoundingBox.Height / 2);
                if (sceneY < sceneBoundingBox.Top)
                {
                    sceneY = sceneBoundingBox.Top;
                }
                else if (sceneY > sceneBoundingBox.Bottom - containerBoundingBox.Height)
                {
                    sceneY = sceneBoundingBox.Bottom - containerBoundingBox.Height;
                }
            }

            Scene.Draw(canvas, -1 * sceneX + offsetX, -1 * sceneY + offsetY);

        }
    }
}
