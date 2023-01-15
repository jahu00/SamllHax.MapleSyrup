using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup.Components
{
    public class MapInstance: DrawableBase, IDrawable, IUpdatable, IBoundable
    {
        private readonly ResourceManager _resourceManager;
        private readonly ComponentHelper _componentHelper;
        private readonly CommonData _commonData;
        private IMap Map { get; set; }
        private DrawableCollection Layers { get; set; }
        private DrawableCollection Portals { get; set; }
        public Sprite Character { get; private set; }
        public SKRectI BoundingBox { get; private set; }

        public MapInstance(ResourceManager resourceManager, ComponentHelper componentHelper, CommonData commonData)
        {
            _resourceManager = resourceManager;
            _componentHelper = componentHelper;
            _commonData = commonData;
        }

        public MapInstance Init(int mapId)
        {
            Map = _resourceManager.GetMap(this, mapId);
            Layers = new DrawableCollection(BuildLayers(Map.Layers));
            Portals = new DrawableCollection(BuildPortals(Map.Portals));
            BoundingBox = Layers.GetBoundingBox();
            Character = new Sprite()
            {
                X = BoundingBox.Left,
                Y = BoundingBox.MidY,
                Bitmap = _resourceManager.GetMobImage(this, "0100100", "stand", "0"),
                //ScaleX = -1,
                OriginX = 18,
                OriginY = 26
            };
            return this;
        }

        private List<IDrawable> BuildPortals(List<IMapPortal> mapPortals)
        {
            /*var portals = mapPortals.Where(x => x.PortalType == PortalType.REGULAR).Select(mapPortal =>
            {
                var portal = _commonData.MapHelpers.Portals.GetEntityByPath<IAnimation>(new string[] { "game", "pv" });
                new AnimatedSprite()
                {
                    X = mapPortal.X,
                    Y = mapPortal.Y,
                };
            });
            return portals;*/
            return new List<IDrawable>();
        }

        private IEnumerable<IDrawable> BuildLayers(IEnumerable<IMapLayer> mapLayers)
        {
            return mapLayers.Select(mapLayer => _componentHelper.CreateMapLayerInstance(this, mapLayer));
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            Layers.Draw(canvas, this.GetTransformMatrix(matrix));
            Character.Draw(canvas, this.GetTransformMatrix(matrix));
            //canvas.DrawRect(new SKRectI(offsetX + BoundingBox.Left, offsetY + BoundingBox.Top, offsetX + BoundingBox.Right, offsetY + BoundingBox.Bottom), new SKPaint() { Color = SKColors.Red, Style = SKPaintStyle.Stroke });
            Portals.Draw(canvas, this.GetTransformMatrix(matrix));
        }

        public void Update(int delta)
        {
            Layers.Update(delta);
            Portals.Update(delta);
        }

        public SKRectI GetBoundingBox()
        {
            return BoundingBox;
        }
    }
}
