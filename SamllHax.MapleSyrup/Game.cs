using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SamllHax.MapleSyrup.Components;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Interfaces.Interfaces.Providers;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup
{
    public class Game : GameWindow, IBoundable
    {
        private readonly ObjectFactory _objectFactory;
        private readonly ComponentHelper _componentHelper;
        private readonly ILogger<Game> _logger;
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;
        private readonly CommonData _commonData;
        private readonly FpsCounter _fpsCounter;
        private readonly GrContextManager _contextManager;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;
        private SKCanvas canvas;

        private float Scale = 1;
        private Vector2i InternalResolution;
        private SKRectI BoundingBox;
        private SKMatrix BaseMatrix;

        //private double timer = 0;

        private MapInstance _mapInstance;
        private SceneCamera<MapInstance> _camera;

        public Game
        (
            IConfiguration configuration,
            ResourceManager resourceManager,
            ObjectFactory objectFactory,
            ComponentHelper componentHelper,
            CommonData commonData,
            FpsCounter fpsCounter,
            GrContextManager contextManager,
            ILogger<Game> logger
        ) : base
        (
            GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (configuration.GetSection("Window").GetValue<int>("Width"), configuration.GetSection("Window").GetValue<int>("Height")), Title = "MyWindow" })
        {
            _logger = logger;
            _configuration = configuration.GetSection("Window");
            _objectFactory = objectFactory;
            _componentHelper = componentHelper;
            _resourceManager = resourceManager;
            _commonData = commonData;
            _fpsCounter = fpsCounter;
            _contextManager = contextManager;
            if (_configuration.GetValue<bool>("VSync"))
            {
                VSync = VSyncMode.On;
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _contextManager.Init();
            _commonData.Init();
            //InitMap(103000000, null);
            //InitMap(103020000, null);
            //InitMap(100000000, null);
            //InitMap(101010103, null);
            InitMap(104000100, null);
        }

        private void InitMap(int mapId, string portalName)
        {
            _logger.LogInformation($"Going to map {mapId} at {portalName}");
            if (_mapInstance != null)
            {
                _resourceManager.AbandonResources(_mapInstance);
            }
            _mapInstance = _componentHelper.CreateMapInstance(mapId, portalName);
            _camera = new SceneCamera<MapInstance>() { Container = this, Scene = _mapInstance, ObjectWithCamera = _mapInstance.Character };
            _resourceManager.FreeResources();
        }

        protected override void OnUnload()
        {
            surface.Dispose();
            renderTarget.Dispose();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            _fpsCounter.PushTime(args.Time);
            //timer += args.Time;
            canvas.Clear(SKColors.CornflowerBlue);
            _camera.Draw(canvas, BaseMatrix);
            canvas.Flush();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            var events = new UpdateEvents((float)args.Time, KeyboardState);
            _mapInstance.Update(events);

            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                // If it is, close the window.
                Close();
            }

            if (KeyboardState.IsKeyPressed(Keys.Space))
            {
                var portalInstance = _mapInstance.Portals.Children.Cast<PortalInstance>().FirstOrDefault(x => x.GetBoundingBox().Contains((int)_mapInstance.Character.X, (int)_mapInstance.Character.Y));
                if (portalInstance != null && portalInstance.MapPortal.TargetMapId < 999999999)
                {
                    if (portalInstance.MapPortal.TargetMapId.ToString() == _mapInstance.Map.Name)
                    {
                        _mapInstance.Character.X = portalInstance.X;
                        _mapInstance.Character.Y = portalInstance.Y;
                    }
                    else
                    {
                        InitMap(portalInstance.MapPortal.TargetMapId, portalInstance.MapPortal.TargetPortalName);
                    }
                }
            }

            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            if (args.Width == 0 || args.Height == 0)
            {
                return;
            }
            renderTarget?.Dispose();
            renderTarget = new GRBackendRenderTarget(args.Width, args.Height, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface?.Dispose();
            surface = SKSurface.Create(_contextManager.GRContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;
            var xScale = args.Width / (float)_configuration.GetValue<int>("Width");
            var yScale = args.Height / (float)_configuration.GetValue<int>("Height");
            Scale = Math.Min(xScale, yScale);
            InternalResolution = new Vector2i(Convert.ToInt32(args.Width / Scale), Convert.ToInt32(args.Height / Scale));
            BoundingBox = new SKRectI(0, 0, InternalResolution.X, InternalResolution.Y);
            BaseMatrix = SKMatrix.CreateScale(Scale, Scale);
            base.OnResize(args);
        }

        public SKRectI GetBoundingBox()
        {
            return BoundingBox;
        }
    }
}
