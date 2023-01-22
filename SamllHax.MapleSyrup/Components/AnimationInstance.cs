using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class AnimationInstance: AnimatedSprite
    {
        public AnimationInstance(IAnimation animationData, Dictionary<string, SKImage> images)
        {
            Frames.AddRange(GetFrames(animationData, images));
            Reset();
        }

        private IEnumerable<Frame> GetFrames(IAnimation animationData, Dictionary<string, SKImage> images)
        {
            foreach (var ( key, frameData ) in animationData.Frames.OrderBy(x => Convert.ToInt32(x.Key)))
            {
                yield return new Frame()
                {
                    Delay = (frameData.Delay ?? Constants.DefaultDelay) / 1000f,
                    Sprite = new Sprite() { Image = images[key], OriginX = frameData.Origin.X, OriginY = frameData.Origin.Y }
                };
            }
        }
    }
}
