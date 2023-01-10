﻿using SkiaSharp;

namespace SamllHax.MapleSyrup.Draw
{
    public class Sprite: DrawableBase, IDrawable, IBoundable
    {
        public SKBitmap Bitmap { get; set; }

        public void Draw(SKCanvas canvas, int x, int y)
        {
            canvas.DrawBitmap(Bitmap, X + x, Y + y);
        }

        public SKRectI GetBoundingBox(int x, int y)
        {
            var offsetX = X + x;
            var offsetY = Y + y;
            var boundingBox = new SKRectI()
            {
                Top = offsetY,
                Left = offsetX,
                Bottom = offsetY + Bitmap.Height,
                Right = offsetX + Bitmap.Width,
            };
            return boundingBox;
        }
    }
}