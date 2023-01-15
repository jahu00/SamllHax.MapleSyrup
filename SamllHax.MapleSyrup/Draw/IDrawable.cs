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
        int OriginX { get; }
        int OriginY { get; }
        float X { get; }
        float Y { get; }
        float ScaleX { get; }
        float ScaleY { get; }
        void Draw(SKCanvas canvas, SKMatrix matrix);
    }

    public static class IDrawableExtensions
    {
        public static SKMatrix GetTransformMatrix(this IDrawable drawable)
        {
            var result = SKMatrix.Identity;
            if (drawable.OriginX != 0 || drawable.OriginY != 0)
            {
                result = result.PostConcat(SKMatrix.CreateTranslation(-1 * drawable.OriginX, -1 * drawable.OriginY));
            }
            if (drawable.ScaleX != 1 || drawable.ScaleY != 1)
            {
                result = result.PostConcat(SKMatrix.CreateScale(drawable.ScaleX, drawable.ScaleY));
            }
            if (drawable.X != 0 || drawable.Y != 0)
            {
                result = result.PostConcat(SKMatrix.CreateTranslation((int)drawable.X, (int)drawable.Y));
            }
            return result;
        }

        public static SKMatrix GetTransformMatrix(this IDrawable drawable, SKMatrix parentMatrix)
        {
            return GetTransformMatrix(drawable).PostConcat(parentMatrix);
        }

        public static SKRectI TransformBoundingBox(this IDrawable drawable, SKRectI boundingBox)
        {
            var tempBoundingBox = new SKRect(boundingBox.Left, boundingBox.Top, boundingBox.Right, boundingBox.Bottom);
            tempBoundingBox.Offset(-1 * drawable.OriginX, -1 * drawable.OriginY);
            tempBoundingBox.Inflate(drawable.ScaleX, drawable.ScaleY);
            tempBoundingBox.Offset(drawable.X, drawable.Y);
            return tempBoundingBox.ToRectI();
        }
    }
}
