using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public interface IUpdatable
    {
        bool Paused { get; }
         void OnUpdate(UpdateEvents events);
    }

    public static class IUpdatableExtensions
    {
        public static void Update(this IUpdatable updatable, UpdateEvents events)
        {
            if (updatable.Paused)
            {
                return;
            }
            updatable.OnUpdate(events);
        }
    }

}
