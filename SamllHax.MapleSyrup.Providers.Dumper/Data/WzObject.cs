using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzObject: WzEntity, IAnimation
    {
        public static readonly string[] NonFrameNodeNames = { "seat", "foothold", "blend", "zigzag" };

        public WzObject(WzDirectory directory) : base(directory)
        {
            foreach (var frameNode in _directory.Children.OrderByDescending(x => x is WzCanvas))
            {
                if (NonFrameNodeNames.Contains(frameNode.Name))
                {
                    continue;
                }
                var frameRepeat = frameNode as WzRepeat;
                if (frameRepeat != null)
                {
                    var repeatedFrame = Frames[frameRepeat.Value];
                    Frames.Add(frameRepeat.Name, repeatedFrame);
                    continue;
                }
                var frameDirectory = frameNode as WzCanvas;
                if (frameDirectory == null)
                {
                    throw new Exception($"Object part is not a canvas, but a {frameNode.GetType().Name}");
                }
                var frame = new WzFrame(frameDirectory);
                Frames.Add(frame.Name, frame);
            }

            Seat = _directory.GetSingleOrDefaultChild<WzDirectory>("seat")?.Children.Select(x => (IVector)x).ToList();
            Foothold = _directory.GetSingleOrDefaultChild<WzExtended>("foothold")?.Children.Select(x => (IVector)x).ToList();
            Blend = _directory.GetSingleOrDefaultChild<WzIntValue>("blend")?.Value;
            ZigZag = _directory.GetSingleOrDefaultChild<WzIntValue>("zigzag")?.Value;
        }

        public Dictionary<string, IFrame> Frames { get; } = new Dictionary<string, IFrame>();
        public List<IVector> Seat { get; }
        public List<IVector> Foothold { get; }
        public int? Blend { get; }
        public int? ZigZag { get; }
    }
}
