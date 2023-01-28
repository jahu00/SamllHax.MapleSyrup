using Logic = SamllHax.MapleSyrup.Logic;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Extensions;
using SamllHax.PlatformerLogic;
using SkiaSharp;
using Microsoft.Extensions.Configuration;

namespace SamllHax.MapleSyrup.Components
{
    public class MapInstance: ComponentBase, IDrawable, IUpdatable, IBoundable
    {
        private readonly IConfigurationSection _configuration;

        public bool DebugMode { get; private set; }

        private readonly ResourceManager _resourceManager;
        private readonly ComponentHelper _componentHelper;
        private readonly CommonData _commonData;
        public IMap MapData { get; private set; }
        public DrawableCollection Layers { get; private set; }
        public DrawableCollection Portals { get; private set; }
        public PlayerInstance Character { get; private set; }
        public SKRectI BoundingBox { get; private set; }

        private SKPaint footholdPaint = new SKPaint() { Color = SKColors.Red };
        private SKPaint boudingBoxPaint = new SKPaint() { Color = SKColors.Red, Style = SKPaintStyle.Stroke };
        public List<Logic.Foothold> Footholds { get; private set; }
        public List<Logic.Foothold> Walls { get; private set; }
        public List<Logic.Foothold> Platforms { get; private set; }

        public int MaxY = int.MinValue;
        public int MinX = int.MaxValue;
        public int MaxX = int.MinValue;

        public MapInstance(IConfiguration configuration, ResourceManager resourceManager, ComponentHelper componentHelper, CommonData commonData)
        {
            _configuration = configuration.GetSection("MapInstance");
            DebugMode = _configuration.GetValue<bool>("Debug");
            _resourceManager = resourceManager;
            _componentHelper = componentHelper;
            _commonData = commonData;
        }

        public MapInstance Init(int mapId, string portalName = null)
        {
            MapData = _resourceManager.GetMap(this, mapId);
            Layers = new DrawableCollection(BuildLayers(MapData.Layers));
            Portals = new DrawableCollection(BuildPortals(MapData.Portals));
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
                var spawnPortals = MapData.Portals.Where(x => x.PortalType == PortalType.SPAWN).ToArray();
                if (spawnPortals.Count() == 0)
                {
                    throw new Exception($"Map {mapId} has no spawn portals");
                }
                var spawnPortal = GlobalRandom.GetRandomElement(spawnPortals);
                Character.X = spawnPortal.X;
                Character.Y = spawnPortal.Y;
            } else
            {
                var spawnPortal = MapData.Portals.SingleOrDefault(x => x.PortalName == portalName);
                if (spawnPortal == null)
                {
                    throw new Exception($"Portal with name {portalName} not found on map {mapId}");
                }
                Character.X = spawnPortal.X;
                Character.Y = spawnPortal.Y;
            }

            Footholds = MapData.Footholds.Select(x => ExtractFootholds(Convert.ToInt32(x.Key), x.Value)).SelectMany(x => x).ToList();
            LinkFootholds(Footholds);
            MaxY = GetMaxY();
            MinX = GetMinX();
            MaxX = GetMaxX();
            Walls = Footholds.Where(x => x.Type == LineType.Vertical).ToList();
            Platforms = Footholds.Where(x => x.Type != LineType.Vertical).ToList();
            return this;
        }

        private int GetMinX()
        {
            return (int)(Footholds.Where(x => x.Type != LineType.Vertical).Min(x => (float?)x.Left) ?? BoundingBox.Left);
        }

        private int GetMaxX()
        {
            return (int)(Footholds.Where(x => x.Type != LineType.Vertical).Max(x => (float?)x.Right) ?? BoundingBox.Right);
        }

        private int GetMaxY()
        {
            return (int)(Footholds.Where(x => x.Type != LineType.Vertical).Max(x => (float?)x.Bottom) ?? BoundingBox.Bottom);
        }

        private void LinkFootholds(List<Logic.Foothold> footholds)
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

        List<Logic.Foothold> ExtractFootholds(int layerId, IEntityDirectory<IMapFoothold> footholdDirectory, int watchdog = 10)
        {
            watchdog--;
            if (watchdog <= 0)
            {
                throw new Exception("Watchdog triggered");
            }
            var result = new List<Logic.Foothold>();
            result.AddRange(footholdDirectory.Directories.Select(x => ExtractFootholds(layerId, x.Value, watchdog)).SelectMany(x => x));
            List<Logic.Foothold> footholds = GetFoodholds(layerId, footholdDirectory.Entities);
            result.AddRange(footholds);
            return result;
        }

        private List<Logic.Foothold> GetFoodholds(int layerId, IDictionary<string,IMapFoothold> entities)
        {
            //var footholdIndex = new Dictionary<int, Foothold>();
            var footholds = entities.Select(x => x.Value).Select
            (
                x =>
                {
                    var foothold = new Logic.Foothold(
                        x1: x.X1,
                        y1: x.Y1,
                        x2: x.X2,
                        y2: x.Y2
                    )
                    {
                        LayerId = layerId,
                        Data = x,
                    };
                    return foothold;
                }
            ).ToList();

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
            var transformedMatrix = this.GetTransformMatrix(matrix);
            for (var layerId = 0; layerId < Constants.LayerCount; layerId++)
            {
                var layer = Layers.Children[layerId];
                layer.Draw(canvas, transformedMatrix);
                if (Character.Z == layerId)
                {
                    Character.Draw(canvas, transformedMatrix);
                }
            }
            Portals.Draw(canvas, transformedMatrix);
            if (DebugMode)
            {
                Footholds.ForEach(foothold => canvas.DrawLine(foothold, footholdPaint, transformedMatrix));
                canvas.DrawRect(BoundingBox, boudingBoxPaint, transformedMatrix);
            }
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
