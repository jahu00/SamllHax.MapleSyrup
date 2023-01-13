using OpenTK.Graphics.ES11;
using SamllHax.MapleSyrup.Extensions;
using SamllHax.MapleSyrup.Interfaces.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public interface IDrawable
    {
        int OffsetX { get; }
        int OffsetY { get; }
        float X { get; }
        float Y { get; }
        float ScaleX { get; }
        float ScaleY { get; }
        void Draw(SKCanvas canvas, SKMatrix matrix);
    }

    public static class IDrawableExtensions
    {
        public static SKMatrix TransformMatrix(this IDrawable drawable, SKMatrix matrix)
        {
            var result = SKMatrix.Identity;
            if (drawable.OffsetX != 0 || drawable.OffsetY != 0)
            {
                result = result.PostConcat(SKMatrix.CreateTranslation(drawable.OffsetX, drawable.OffsetY));
            }
            if (drawable.ScaleX != 1 || drawable.ScaleY != 1)
            {
                result = result.PostConcat(SKMatrix.CreateScale(drawable.ScaleX, drawable.ScaleY));
            }
            if (drawable.X != 0 || drawable.Y != 0)
            {
                result = result.PostConcat(SKMatrix.CreateTranslation((int)drawable.X, (int)drawable.Y));
            }
            result = result.PostConcat(matrix);
            return result;
        }

        public static SKRectI TransformBoundingBox(this IDrawable drawable, SKRectI boundingBox)
        {
            var tempBoundingBox = new SKRect(boundingBox.Left, boundingBox.Top, boundingBox.Right, boundingBox.Bottom);
            tempBoundingBox.Offset(drawable.OffsetX, drawable.OffsetY);
            tempBoundingBox.Inflate(drawable.ScaleX, drawable.ScaleY);
            tempBoundingBox.Offset(drawable.X, drawable.Y);
            return tempBoundingBox.ToRectI();
        }
    }
}
