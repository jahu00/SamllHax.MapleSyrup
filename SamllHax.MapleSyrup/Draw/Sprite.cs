using SamllHax.MapleSyrup.Extensions;
using SkiaSharp;

namespace SamllHax.MapleSyrup.Draw
{
    public class Sprite: DrawableBase, IDrawable, IBoundable
    {
        public SKImage Image { get; set; }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            canvas.DrawImage(Image, this.GetTransformMatrix(matrix));
        }

        public SKRectI GetBoundingBox()
        {
            return this.TransformBoundingBox(Image.GetBoundingBox());
        }
    }
}
