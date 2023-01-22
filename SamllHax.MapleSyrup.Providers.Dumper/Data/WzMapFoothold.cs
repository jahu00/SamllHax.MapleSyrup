using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapFoothold : WzEntity, IMapFoothold
    {
        public WzMapFoothold(WzDirectory directory) : base(directory)
        {
            foreach(var node in directory.Children)
            {
                var intNode = node as WzIntValue;
                if (intNode == null)
                {
                    throw new Exception($"Node {node.Name} is not of type WzIntValue, found {node.GetType().Name} instead");
                }
                switch (node.Name)
                {
                    case "x1":
                        X1 = intNode.Value;
                        break;
                    case "x2":
                        X2 = intNode.Value;
                        break;
                    case "y1":
                        Y1 = intNode.Value;
                        break;
                    case "y2":
                        Y2 = intNode.Value;
                        break;
                    case "prev":
                        Prev = intNode.Value;
                        break;
                    case "next":
                        Next = intNode.Value;
                        break;
                    default:
                        throw new Exception($"Unsupported {node.Name} of type {node.GetType().Name}");
                }
            }
        }

        public int X1 { get; }

        public int Y1 { get; }

        public int X2 { get; }

        public int Y2 { get; }

        public int Prev { get; }

        public int Next { get; }
    }
}
