using OpenTK.Windowing.Common;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Interfaces.Data;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class PortalInstance: ComponentBase, IDrawable, IBoundable, IUpdatable
    {
        public IDrawable Drawable { get; private set; }
        public IMapPortal MapPortal { get; }
        public PortalInstance(IMapPortal mapPortal, IDrawable drawable): base()
        {
            MapPortal = mapPortal;
            Drawable = drawable;
        }

        public void OnUpdate(UpdateEvents events)
        {
            var updatable = Drawable as IUpdatable;
            updatable?.Update(events);
        }

        public SKRectI GetBoundingBox()
        {
            return this.TransformBoundingBox(Constants.PortalBoundingBox);
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            Drawable?.Draw(canvas, this.GetTransformMatrix(matrix));
        }
    }
}
