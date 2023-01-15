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
        public int FrameId { get; private set; }
        public int Timer { get; private set; }
        public List<Frame> Frames { get; set; } = new List<Frame>();
        public AnimationType AnimationType { get; set; } = AnimationType.NormalLoop;
        public bool Complete { get; private set; }
        public short Direction { get; private set; }


        public AnimatedSprite()
        {
            Reset();
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            var currentFrame = Frames[FrameId];
            currentFrame.Sprite.Draw(canvas, this.GetTransformMatrix(matrix));
        }

        public SKRectI GetBoundingBox()
        {
            var currentFrame = Frames[FrameId];
            return this.TransformBoundingBox(currentFrame.Sprite.GetBoundingBox());
        }

        public void Update(int delta)
        {
            if (Complete || delta <= 0)
            {
                return;
            }
            if (Frames.Count == 0 && AnimationType != AnimationType.SinglePass) {
                return;
            }
            var currentFrame = Frames[FrameId];
            Timer += delta;
            if (currentFrame.Delay > Timer)
            {
                return;
            }
            Timer -= currentFrame.Delay;
            FrameId += Direction;
            if (AnimationType == AnimationType.SinglePass)
            {
                FrameId -= Direction;
                Timer = currentFrame.Delay;
                Complete = true;
                return;
            }
            if (FrameId < Frames.Count || FrameId < 0)
            {
                return;
            }
            if (AnimationType == AnimationType.NormalLoop)
            {
                FrameId = 0;
                return;
            }
            
            if (AnimationType == AnimationType.ZiZagLoop)
            {
                Direction *= -1;
                FrameId += Direction * 2;
            }
        }

        public void Reset()
        {
            FrameId = 0;
            Timer = 0;
            Complete = false;
            Direction = 1;
        }
    }
}
