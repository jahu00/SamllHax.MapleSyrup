using System;
using System.Collections.Generic;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IMapPortal: IMapEntityBase
    {
        string PortalName { get; }
        PortalType PortalType { get; }
        int TargetMapId { get; }
        string TargetPortalName { get; }
        string Image { get; }
    }
}
