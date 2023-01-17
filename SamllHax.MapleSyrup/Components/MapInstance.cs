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
        public IMap Map { get; private set; }
        public DrawableCollection Layers { get; private set; }
        public DrawableCollection Portals { get; private set; }
        public Sprite Character { get; private set; }
        public SKRectI BoundingBox { get; private set; }

        public MapInstance(ResourceManager resourceManager, ComponentHelper componentHelper, CommonData commonData)
        {
            _resourceManager = resourceManager;
            _componentHelper = componentHelper;
            _commonData = commonData;
        }

        public MapInstance Init(int mapId, string portalName = null)
        {
            Map = _resourceManager.GetMap(this, mapId);
            Layers = new DrawableCollection(BuildLayers(Map.Layers));
            Portals = new DrawableCollection(BuildPortals(Map.Portals));
            BoundingBox = Layers.GetBoundingBox();
            Character = new Sprite()
            {
                /*X = BoundingBox.Left,
                Y = BoundingBox.MidY,*/
                Bitmap = _resourceManager.GetMobImage(this, "0100100", "stand", "0"),
                //ScaleX = -1,
                OriginX = 18,
                OriginY = 26
            };
            if (string.IsNullOrEmpty(portalName))
            {
                var spawnPortals = Map.Portals.Where(x => x.PortalType == PortalType.SPAWN).ToArray();
                if (spawnPortals.Count() == 0)
                {
                    throw new Exception($"Map {mapId} has no spawn portals");
                }
                var spawnPortal = GlobalRandom.GetRandomElement(spawnPortals);
                Character.X = spawnPortal.X;
                Character.Y = spawnPortal.Y;
            } else
            {
                var spawnPortal = Map.Portals.SingleOrDefault(x => x.PortalName == portalName);
                if (spawnPortal == null)
                {
                    throw new Exception($"Portal with name {portalName} not found on map {mapId}");
                }
                Character.X = spawnPortal.X;
                Character.Y = spawnPortal.Y;
            }
            return this;
        }

        private IEnumerable<IDrawable> BuildPortals(List<IMapPortal> mapPortals)
        {
            return mapPortals.Where(x => x.PortalType == PortalType.REGULAR || x.PortalType == PortalType.HIDDEN).Select(mapPortal => (IDrawable)_componentHelper.CreatePortalInstance(mapPortal));
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
