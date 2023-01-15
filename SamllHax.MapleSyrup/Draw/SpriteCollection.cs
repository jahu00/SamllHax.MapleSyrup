using OpenTK.Graphics.OpenGL;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class DrawableCollection: DrawableBase, IDrawable, IUpdatable, IBoundable
    {
        public List<IDrawable> Children { get; } = new List<IDrawable>();

        public DrawableCollection()
        {
        }

        public DrawableCollection(IEnumerable<IDrawable> drawables)
        {
            Children.AddRange(drawables);
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            foreach(var drawable in Children)
            {
                drawable.Draw(canvas, this.GetTransformMatrix(matrix));
            }

        }

        public SKRectI GetBoundingBox()
        {
            var top = 0;
            var left = 0;
            var bottom = 0;
            var right = 0;
            foreach (var sprite in Children)
            {
                if (sprite is not IBoundable)
                {
                    continue;
                }
                var boundable = (IBoundable)sprite;
                var boudingBox = boundable.GetBoundingBox();
                if (boudingBox.Top < top)
                {
                    top = boudingBox.Top;
                }
                if (boudingBox.Left < left)
                {
                    left = boudingBox.Left;
                }
                if (boudingBox.Bottom > bottom)
                {
                    bottom = boudingBox.Bottom;
                }
                if (boudingBox.Right > right)
                {
                    right = boudingBox.Right;
                }
            }
            return this.TransformBoundingBox(new SKRectI() { Top = top, Left = left, Bottom = bottom, Right = right });
        }

        public void Update(int delta)
        {
            foreach (var sprite in Children)
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
