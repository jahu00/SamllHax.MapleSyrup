using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class AnimatedSprite : DrawableBase, IDrawable, IUpdatable, IBoundable
    {
        public int FrameId { get; private set; } = 0;
        public int Timer { get; private set; } = 0;
        public List<Frame> Frames { get; set; }


        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            var currentFrame = Frames[FrameId];
            currentFrame.Sprite.Draw(canvas, this.TransformMatrix(matrix));
        }

        public SKRectI GetBoundingBox()
        {
            var currentFrame = Frames[FrameId];
            return this.TransformBoundingBox(currentFrame.Sprite.GetBoundingBox());
        }

        public void Update(int delta)
        {
            if (delta < 0)
            {
                return;
            }
            Frame currentFrame;
            /*do
            {*/
                currentFrame = Frames[FrameId];
                Timer += delta;
                if (currentFrame.Delay > Timer)
                {
                    return;
                }
                Timer -= currentFrame.Delay;
                FrameId++;
                if (FrameId >= Frames.Count)
                {
                    FrameId = 0;
                }
            /*} while (Timer >= currentFrame.Delay);*/
        }
    }
}
