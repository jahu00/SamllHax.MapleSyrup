using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamllHax.MapleSyrup.Providers.Dumper.Data
{
    public class WzPhysics : WzEntity, IPhysics
    {
        public WzPhysics(WzDirectory directory) : base(directory)
        {
            var type = typeof(WzPhysics);
            var properties = type.GetProperties();
            foreach(var node in directory.Children.Cast<WzFloatValue>())
            {
                var property = properties.Single(x => x.Name.Equals(node.Name, StringComparison.CurrentCultureIgnoreCase));
                if (property == null)
                {
                    throw new Exception($"WzPhysics has no property named {node.Name}");
                }

                property.SetValue(this, node.Value, null);
            }
        }

        public float WalkForce { get; set; }

        public float WalkSpeed { get; set; }

        public float WalkDrag { get; set; }

        public float SlipForce { get; set; }

        public float SlipSpeed { get; set; }

        public float FloatDrag1 { get; set; }

        public float FloatDrag2 { get; set; }

        public float FloatCoefficient { get; set; }

        public float SwimForce { get; set; }

        public float SwimSpeed { get; set; }

        public float FlyForce { get; set; }

        public float FlySpeed { get; set; }

        public float GravityAcc { get; set; }

        public float FallSpeed { get; set; }

        public float JumpSpeed { get; set; }

        public float MaxFriction { get; set; }

        public float MinFriction { get; set; }

        public float SwimSpeedDec { get; set; }

        public float FlyJumpDec { get; set; }
    }
}
