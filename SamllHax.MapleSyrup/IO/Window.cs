using Microsoft.Extensions.Configuration;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SamllHax.MapleSyrup.Data;
using SkiaSharp;

namespace SamllHax.MapleSyrup.IO
{
    public class Window: GameWindow
    {
        private GRGlInterface grgInterface;
        private GRContext grContext;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;
        private SKCanvas canvas;
        private readonly IConfiguration _windowConfiguration;
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;
        private float Scale = 1;
        private Vector2i InternalResolution;


        private double timer = 0;

        private MapInstance _mapInstance;

        public Window(IConfiguration configuration, ResourceManager resourceManager): base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (configuration.GetSection("Window").GetValue<int>("Width"), configuration.GetSection("Window").GetValue<int>("Height")), Title = "MyWindow" })
        {
            _configuration = configuration;
            _windowConfiguration = configuration.GetSection("Window");
            _resourceManager = resourceManager;
            VSync = VSyncMode.On;
            InternalResolution = new Vector2i(Size.X, Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //Context.MakeCurrent();
            grgInterface = GRGlInterface.Create();
            grContext = GRContext.CreateGl(grgInterface);
            renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;

            _mapInstance = new MapInstance(_resourceManager, 100000000);
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
            int mapX;
            int mapY;
            if (_mapInstance.BoudingBox.Width <= InternalResolution.X)
            {
                mapX = _mapInstance.BoudingBox.MidX - (InternalResolution.X / 2);
            }
            else
            {
                mapX = _mapInstance.Character.X - (InternalResolution.X / 2);
                if (mapX < _mapInstance.BoudingBox.Left)
                {
                    mapX = _mapInstance.BoudingBox.Left;
                }
                else if (mapX > _mapInstance.BoudingBox.Right - InternalResolution.X)
                {
                    mapX = _mapInstance.BoudingBox.Right - InternalResolution.X;
                }
            }

            if (_mapInstance.BoudingBox.Height <= InternalResolution.Y)
            {
                mapY = _mapInstance.BoudingBox.MidY - (InternalResolution.Y / 2);
            }
            else
            {
                mapY = _mapInstance.Character.Y - (InternalResolution.Y / 2);
                if (mapY < _mapInstance.BoudingBox.Top)
                {
                    mapY = _mapInstance.BoudingBox.Top;
                }
                else if (mapY > _mapInstance.BoudingBox.Bottom - InternalResolution.Y)
                {
                    mapY = _mapInstance.BoudingBox.Bottom - InternalResolution.Y;
                }
            }

            _mapInstance.Draw(canvas, -1 * mapX, -1 * mapY);
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

            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                // If it is, close the window.
                Close();
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                _mapInstance.Character.Y += 1;
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                _mapInstance.Character.Y -= 1;
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                _mapInstance.Character.X -= 1;
            }
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                _mapInstance.Character.X += 1;
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            renderTarget = new GRBackendRenderTarget(args.Width, args.Height, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            //renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            //renderTarget = new GRBackendRenderTarget(Size.X, Size.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;
            var xScale = (float)args.Width / (float)_windowConfiguration.GetValue<int>("Width");
            var yScale = (float)args.Height / (float)_windowConfiguration.GetValue<int>("Height");
            Scale = Math.Min(xScale, yScale);
            InternalResolution = new Vector2i(Convert.ToInt32(args.Width / Scale), Convert.ToInt32(args.Height / Scale));
            canvas.Scale(Scale);
            base.OnResize(args);
        }
    }
}
