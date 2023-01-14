﻿using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzMapPortal : WzEntity
    {
        public WzMapPortal(WzDirectory directory) : base(directory)
        {
            X = _directory.GetSingleChild<WzIntValue>("x").Value;
            Y = _directory.GetSingleChild<WzIntValue>("y").Value;
            PortalType = (PortalType)_directory.GetSingleChild<WzIntValue>("pt").Value;
            PortalName = _directory.GetSingleChild<WzStringValue>("pn").Value;
            TargetMapId = _directory.GetSingleChild<WzStringValue>("tm").Value;
            TargetPortalName = _directory.GetSingleChild<WzStringValue>("tn").Value;
        }

        public int X { get; }
        public int Y { get; }
        public string PortalName { get; }
        public PortalType PortalType { get; }
        public string TargetMapId { get; }
        public string TargetPortalName { get; }
    }
}
