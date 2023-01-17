using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapPortal : WzEntity, IMapPortal
    {
        public WzMapPortal(WzDirectory directory) : base(directory)
        {
            X = _directory.GetSingleChild<WzIntValue>("x").Value;
            Y = _directory.GetSingleChild<WzIntValue>("y").Value;
            PortalType = (PortalType)_directory.GetSingleChild<WzIntValue>("pt").Value;
            PortalName = _directory.GetSingleChild<WzStringValue>("pn").Value;
            TargetMapId = _directory.GetSingleChild<WzIntValue>("tm").Value;
            TargetPortalName = _directory.GetSingleChild<WzStringValue>("tn").Value;
            Image = _directory.GetSingleOrDefaultChild<WzStringValue>("image")?.Value;
        }

        public int X { get; }
        public int Y { get; }
        public string PortalName { get; }
        public PortalType PortalType { get; }
        public int TargetMapId { get; }
        public string TargetPortalName { get; }
        public string Image { get; }
    }
}
