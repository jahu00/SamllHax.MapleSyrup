using OpenTK.Windowing.Common;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup.Components
{
    public class MapInstance: ComponentBase, IDrawable, IUpdatable, IBoundable
    {
        private readonly ResourceManager _resourceManager;
        private readonly ComponentHelper _componentHelper;
        private readonly CommonData _commonData;
        public IMap Map { get; private set; }
        public DrawableCollection Layers { get; private set; }
        public DrawableCollection Portals { get; private set; }
        public PlayerInstance Character { get; private set; }
        public SKRectI BoundingBox { get; private set; }

        private SKPaint FootholdPaint { get; set; } = new SKPaint() { Color = SKColors.Red };
        public List<Foothold> Footholds { get; private set; }

        public int MaxY = int.MinValue;
        public int MinX = int.MaxValue;
        public int MaxX = int.MinValue;

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
            var characterSprite = new Sprite()
            {
                /*X = BoundingBox.Left,
                Y = BoundingBox.MidY,*/
                Image = _resourceManager.GetMobImage(this, "0100100", "stand", "0"),
                //ScaleX = -1,
                OriginX = 18,
                OriginY = 26
            };

            Character = _componentHelper.CreatePlayerInstance(characterSprite, this);
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

            Footholds = Map.Footholds.Select(x => ExtractFootholds(Convert.ToInt32(x.Key), x.Value)).SelectMany(x => x).ToList();
            LinkFootholds(Footholds);
            MaxY = GetMaxY();
            MinX = GetMinX();
            MaxX = GetMaxX();
            return this;
        }

        private int GetMinX()
        {
            return (int)(Footholds.Where(x => !x.IsVertical).Min(x => (float?)x.BoudingBox.Left) ?? BoundingBox.Left);
        }

        private int GetMaxX()
        {
            return (int)(Footholds.Where(x => !x.IsVertical).Max(x => (float?)x.BoudingBox.Right) ?? BoundingBox.Right);
        }

        private int GetMaxY()
        {
            return (int)(Footholds.Where(x => !x.IsVertical).Max(x => (float?)x.BoudingBox.Bottom) ?? BoundingBox.Bottom);
        }

        private void LinkFootholds(List<Foothold> footholds)
        {
            var footholdIndex = footholds.ToDictionary(x => Convert.ToInt32(x.Data.Name), x => x);
            footholds.ForEach(x =>
            {
                if (x.Data.Next != 0 && footholdIndex.TryGetValue(x.Data.Next, out var nextFoothold))
                {
                    x.Next = nextFoothold;
                }
                if (x.Data.Previous != 0 && footholdIndex.TryGetValue(x.Data.Previous, out var previousFoothold))
                {
                    x.Previous = previousFoothold;
                }
            });
        }

        public MatchingFoothold[] GetFootholdsForX(float x)
        {
            var footholds = Footholds.Where(foothold => !foothold.IsVertical && foothold.ContainsHorizontally(x)).Select(foothold => new MatchingFoothold() { Foothold = foothold, Y = foothold.GetY(x)}).OrderBy(pair => pair.Y).ToArray();
            if (footholds.Count() > 0)
            {
                footholds.Last().IsLast = true;
            }
            return footholds;
        }

        List<Foothold> ExtractFootholds(int layerId, IEntityDirectory<IMapFoothold> footholdDirectory, int watchdog = 10)
        {
            watchdog--;
            if (watchdog <= 0)
            {
                throw new Exception("Watchdog triggered");
            }
            var result = new List<Foothold>();
            result.AddRange(footholdDirectory.Directories.Select(x => ExtractFootholds(layerId, x.Value, watchdog)).SelectMany(x => x));
            List<Foothold> footholds = GetFoodholds(layerId, footholdDirectory.Entities);
            result.AddRange(footholds);
            return result;
        }

        private List<Foothold> GetFoodholds(int layerId, IDictionary<string,IMapFoothold> entities)
        {
            //var footholdIndex = new Dictionary<int, Foothold>();
            var footholds = entities.Select(x => x.Value).Select
            (
                x =>
                {
                    var foothold = new Foothold(
                        x1: x.X1,
                        y1: x.Y1,
                        x2: x.X2,
                        y2: x.Y2
                    )
                    {
                        LayerId = layerId,
                        Data = x,
                        Paint = FootholdPaint
                    };
                    //footholdIndex.Add(Convert.ToInt32(x.Name), foothold);
                    return foothold;
                }
            ).ToList();

            /*footholds.ForEach(x =>
            {
                if (x.Data.Next != 0 && footholdIndex.TryGetValue(x.Data.Next, out var nextFoothold))
                {
                    x.Next = nextFoothold;
                }
                if (x.Data.Previous != 0 && footholdIndex.TryGetValue(x.Data.Previous, out var previousFoothold))
                {
                    x.Previous = previousFoothold;
                }
            });*/
            return footholds;
        }

        private IEnumerable<IDrawable> BuildPortals(List<IMapPortal> mapPortals)
        {
            return mapPortals.Select(mapPortal => _componentHelper.CreatePortalInstance(mapPortal));
        }

        private IEnumerable<IDrawable> BuildLayers(IEnumerable<IMapLayer> mapLayers)
        {
            return mapLayers.Select(mapLayer => _componentHelper.CreateMapLayerInstance(this, mapLayer));
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            for (var layerId = 0; layerId < Constants.LayerCount; layerId++)
            {
                var layer = Layers.Children[layerId];
                layer.Draw(canvas, this.GetTransformMatrix(matrix));
                if (Character.Z == layerId)
                {
                    Character.Draw(canvas, this.GetTransformMatrix(matrix));
                }
            }
            //Layers.Draw(canvas, this.GetTransformMatrix(matrix));
            //canvas.DrawRect(new SKRectI(offsetX + BoundingBox.Left, offsetY + BoundingBox.Top, offsetX + BoundingBox.Right, offsetY + BoundingBox.Bottom), new SKPaint() { Color = SKColors.Red, Style = SKPaintStyle.Stroke });
            Portals.Draw(canvas, this.GetTransformMatrix(matrix));
            Footholds.ForEach(foothold => foothold.Draw(canvas, matrix));
        }

        public void OnUpdate(UpdateEvents events)
        {
            Layers.Update(events);
            Portals.Update(events);
            Character.Update(events);
        }

        public SKRectI GetBoundingBox()
        {
            return BoundingBox;
        }
    }
}
