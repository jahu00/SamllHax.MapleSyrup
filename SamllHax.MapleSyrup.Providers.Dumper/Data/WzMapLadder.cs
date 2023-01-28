using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapLadder : WzEntity, IMapLadder
    {
        public WzMapLadder(WzDirectory directory) : base(directory)
        {
            foreach (var node in directory.Children)
            {
                var intNode = node as WzIntValue;
                if (intNode == null)
                {
                    throw new Exception($"Node {node.Name} is not of type WzIntValue, found {node.GetType().Name} instead");
                }
                switch (node.Name)
                {
                    case "x":
                        X = intNode.Value;
                        break;
                    case "y1":
                        Y1 = intNode.Value;
                        break;
                    case "y2":
                        Y2 = intNode.Value;
                        break;
                    case "l":
                        Type = (LadderType)intNode.Value;
                        break;
                    case "page":
                        LayerId = intNode.Value;
                        break;
                    case "uf":
                        CanExitUp = intNode.Value == 1;
                        break;
                    default:
                        throw new Exception($"Unsupported {node.Name} of type {node.GetType().Name}");
                }
            }
        }

        public LadderType Type { get; }
        public bool CanExitUp { get; }
        public int X { get; }
        public int Y1 { get; }
        public int Y2 { get; }
        public int LayerId { get; }
    }
}
