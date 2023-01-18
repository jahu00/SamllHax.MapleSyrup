using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzAnimation: WzEntity, IAnimation
    {
        public static readonly string[] NonFrameNodeNames = { "seat", "foothold", "blend", "zigzag", "obstacle", "ladder", "rope", "repeat", "origin" };

        public WzAnimation(WzDirectory directory) : base(directory)
        {
            foreach (var pair in _directory.Children.Select((x, i) => new { Node = x, Index = i  }).OrderByDescending(x => x.Node is WzCanvas))
            {
                var frameNode = pair.Node;
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
                /*var intRepeat = frameNode as WzIntValue;
                if (intRepeat != null && intRepeat.Name == "repeat")
                {
                    var repeatedFrame = Frames[frameRepeat.Value];
                    Frames.Add(frameRepeat.Name, repeatedFrame);
                    continue;
                }*/
                var frameDirectory = frameNode as WzCanvas;
                if (frameDirectory == null)
                {
                    throw new Exception($"Object part is not a canvas, but a {frameNode.GetType().Name}");
                }
                var frame = new WzFrame(frameDirectory);
                Frames.Add(frame.Name, frame);
            }

            Seat = _directory.GetVectorCollections("seat");
            Foothold = _directory.GetVectorCollections("foothold");
            Ladder = _directory.GetVectorCollections("ladder");
            Ladder = _directory.GetVectorCollections("rope");
            Blend = _directory.GetSingleOrDefaultChild<WzIntValue>("blend")?.Value;
            ZigZag = _directory.GetSingleOrDefaultChild<WzIntValue>("zigzag")?.Value;
            Obstacle = _directory.GetSingleOrDefaultChild<WzIntValue>("obstacle")?.Value;
            Origin = _directory.GetSingleOrDefaultChild<WzVector>("origin");
        }

        public Dictionary<string, IFrame> Frames { get; } = new Dictionary<string, IFrame>();
        public List<IVectorCollection> Seat { get; }
        public List<IVectorCollection> Foothold { get; }
        public List<IVectorCollection> Ladder { get; }
        public List<IVectorCollection> Rope { get; }
        public int? Blend { get; }
        public int? ZigZag { get; }
        public int? Obstacle { get; }
        public IVector Origin { get; }
    }
}
