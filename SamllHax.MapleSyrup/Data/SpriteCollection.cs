using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class SpriteCollection: DrawableBase, IDrawable, IUpdatable, IBoundable
    {
        public List<IDrawable> Sprites { get; set; } = new List<IDrawable>();

        public void Draw(SKCanvas canvas, int x, int y)
        {
            var offsetX = X + x;
            var offsetY = Y + y;
            foreach(var sprite in Sprites)
            {
                sprite.Draw(canvas, offsetX, offsetY);
            }

        }

        public SKRect GetBoundingBox(int x, int y)
        {
            var offsetX = X + x;
            var offsetY = Y + y;
            float top = offsetY;
            float left = offsetX;
            float bottom = offsetY;
            float right = offsetX;
            foreach (var sprite in Sprites)
            {
                if (sprite is not IBoundable)
                {
                    continue;
                }
                var boundable = (IBoundable)sprite;
                var boudingBox = boundable.GetBoundingBox(offsetX, offsetY);
                if (boudingBox.Top < top)
                {
                    top = boudingBox.Top;
                }
                if (boudingBox.Left < left)
                {
                    left = boudingBox.Left;
                }
                if (boudingBox.Bottom < bottom)
                {
                    right = boudingBox.Right;
                }
                if (boudingBox.Right < right)
                {
                    right = boudingBox.Right;
                }
            }
            return new SKRect() { Top = top, Left = left, Bottom = bottom, Right = right };
        }

        public void Update(int delta)
        {
            foreach (var sprite in Sprites)
            {
                if (sprite is not IUpdatable)
                {
                    continue;
                }
                var updatable = (IUpdatable)sprite;
                updatable.Update(delta);
            }
        }
    }
}
