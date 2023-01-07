using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzObjectPart: WzEntity
    {
        public static readonly string[] NonFrameNodeNames = { "seat", "foothold", "blend", "zigzag" };

        public WzObjectPart(WzDirectory directory) : base(directory)
        {
            foreach (var frameNode in _directory.Children)
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
                Frames.Add(frame.Id, frame);
            }
        }

        public Dictionary<string, WzFrame> Frames { get; } = new Dictionary<string, WzFrame>();
        public List<WzVector> Seat => _directory.GetSingleOrDefaultChild<WzExtended>("seat")?.Children.Select(x => (WzVector)x).ToList();
        public List<WzVector> Foothold => _directory.GetSingleOrDefaultChild<WzExtended>("foothold")?.Children.Select(x => (WzVector)x).ToList();
        public int? Blend => _directory.GetSingleOrDefaultChild<WzIntValue>("blend")?.Value;
        public int? ZigZag => _directory.GetSingleOrDefaultChild<WzIntValue>("zigzag")?.Value;
    }
}
