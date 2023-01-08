using Microsoft.Extensions.Configuration;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GLFW;

namespace SamllHax.MapleSyrup.IO
{
    public class Window: GameWindow
    {
        private GRGlInterface grgInterface;
        private GRContext grContext;
        private GRBackendRenderTarget renderTarget;
        private SKSurface surface;
        private SKCanvas canvas;
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;

        private double timer = 0;

        private MapInstance _mapInstance;

        public Window(IConfiguration configuration, ResourceManager resourceManager): base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (configuration.GetSection("Window").GetValue<int>("Width"), configuration.GetSection("Window").GetValue<int>("Height")), Title = "MyWindow" })
        {
            _configuration = configuration;
            _resourceManager = resourceManager;
            VSync = VSyncMode.On;
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
            _mapInstance.Draw(canvas, 0, 0);
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

            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            /*renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;*/
            base.OnResize(args);
        }
    }
}
