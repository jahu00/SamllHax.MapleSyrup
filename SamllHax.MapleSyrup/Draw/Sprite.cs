﻿using SamllHax.MapleSyrup.Extensions;
using SkiaSharp;

namespace SamllHax.MapleSyrup.Draw
{
    public class Sprite: DrawableBase, IDrawable, IBoundable
    {
        public SKBitmap Bitmap { get; set; }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            canvas.DrawBitmap(Bitmap, this.GetTransformMatrix(matrix));
        }

        public SKRectI GetBoundingBox()
        {
            return this.TransformBoundingBox(Bitmap.GetBoundingBox());
        }
    }
}
