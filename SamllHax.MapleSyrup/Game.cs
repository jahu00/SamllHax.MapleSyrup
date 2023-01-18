using Microsoft.Extensions.Configuration;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SamllHax.MapleSyrup.Components;
using SamllHax.MapleSyrup.Draw;
using SamllHax.MapleSyrup.Helpers;
using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup
{
    public class Game : GameWindow, IBoundable
    {
        private readonly IConfiguration _windowConfiguration;
        private readonly ObjectFactory _objectFactory;
        private readonly ComponentHelper _componentHelper;
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;
        private readonly CommonData _commonData;

        private GRGlInterface grgInterface;
        private GRContext grContext;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;
        private SKCanvas canvas;

        private float Scale = 1;
        private Vector2i InternalResolution;
        private SKRectI BoundingBox;
        private SKMatrix BaseMatrix;

        private double timer = 0;

        private MapInstance _mapInstance;
        private SceneCamera<MapInstance> _camera;

        public Game
        (
            IConfiguration configuration,
            ResourceManager resourceManager,
            ObjectFactory objectFactory,
            ComponentHelper componentHelper,
            CommonData commonData
        ) : base(/*new GameWindowSettings() { RenderFrequency = 75, UpdateFrequency = 60 }*/ GameWindowSettings.Default, new NativeWindowSettings() { Size = (configuration.GetSection("Window").GetValue<int>("Width"), configuration.GetSection("Window").GetValue<int>("Height")), Title = "MyWindow" })
        {
            _configuration = configuration;
            _windowConfiguration = configuration.GetSection("Window");
            _objectFactory = objectFactory;
            _componentHelper = componentHelper;
            _resourceManager = resourceManager;
            _commonData = commonData;
            VSync = VSyncMode.On;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            grgInterface = GRGlInterface.Create();
            grContext = GRContext.CreateGl(grgInterface);
            InitMap(100000000, null);
        }

        private void InitMap(int mapId, string portalName)
        {
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
            grContext.Dispose();
            grgInterface.Dispose();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //var delta = Convert.ToInt32(args.Time * 1000d);
            //timer += args.Time;
            canvas.Clear(SKColors.CornflowerBlue);
            _camera.Draw(canvas, BaseMatrix);
            canvas.Flush();
            SwapBuffers();
            var fps = Convert.ToInt32(1 / args.Time);
            Title = $"FPS: {fps}";
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            var delta = Convert.ToInt32(args.Time * 1000d);
            timer += args.Time;
            _mapInstance.Update(delta);

            var speed = 500;
            var move = (float)(speed * args.Time);

            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                // If it is, close the window.
                Close();
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                _mapInstance.Character.Y += move;
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                _mapInstance.Character.Y -= move;
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                _mapInstance.Character.X -= move;
                _mapInstance.Character.ScaleX = 1;
            }
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                _mapInstance.Character.X += move;
                _mapInstance.Character.ScaleX = -1;
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
            renderTarget?.Dispose();
            renderTarget = new GRBackendRenderTarget(args.Width, args.Height, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface?.Dispose();
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;
            var xScale = args.Width / (float)_windowConfiguration.GetValue<int>("Width");
            var yScale = args.Height / (float)_windowConfiguration.GetValue<int>("Height");
            Scale = Math.Min(xScale, yScale);
            InternalResolution = new Vector2i(Convert.ToInt32(args.Width / Scale), Convert.ToInt32(args.Height / Scale));
            BoundingBox = new SKRectI(0, 0, InternalResolution.X, InternalResolution.Y);
            //canvas.Scale(Scale);
            BaseMatrix = SKMatrix.CreateScale(Scale, Scale);
            base.OnResize(args);
        }

        public SKRectI GetBoundingBox()
        {
            return BoundingBox;
        }
    }
}
