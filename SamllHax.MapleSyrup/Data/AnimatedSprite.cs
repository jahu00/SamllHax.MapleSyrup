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


        public void Draw(SKCanvas canvas, int x, int y)
        {
            var currentFrame = Frames[FrameId];
            var offsetX = X + x;
            var offsetY = Y + y;
            currentFrame.Sprite.Draw(canvas, offsetX, offsetY);
        }

        public SKRect GetBoundingBox(int x, int y)
        {
            var offsetX = X + x;
            var offsetY = Y + y;
            var currentFrame = Frames[FrameId];
            return currentFrame.Sprite.GetBoundingBox(offsetX, offsetY);
        }

        public void Update(int delta)
        {
            Frame currentFrame;
            do
            {
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
            } while (Timer >= currentFrame.Delay);
        }
    }
}
