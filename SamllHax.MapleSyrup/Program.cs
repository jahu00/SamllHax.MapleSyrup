using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SamllHax.MapleSyrup.Interfaces.Providers;
using SamllHax.MapleSyrup.Providers.Dumper;
using SamllHax.MapleSyrup.Helpers;
using Microsoft.Extensions.Logging;
using SamllHax.PlatformerLogic;

namespace SamllHax.MapleSyrup
{
    public static class Program
    {
        public static void Main()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<CollisionDetector>();
                    services.AddSingleton<FpsCounter>();
                    services.AddSingleton<GrContextManager>();
                    services.AddSingleton<IResourceProvider, DumperResourceProvider>();
                    services.AddSingleton<ObjectFactory>();
                    services.AddSingleton<ResourceManager>();
                    services.AddSingleton<ComponentHelper>();
                    services.AddSingleton<CommonData>();
                    //services.AddSingleton<MapRenderer>();
                    services.AddSingleton<Game>();
                });

            hostBuilder.ConfigureLogging(logging => { logging.AddConsole(); });

            var host = hostBuilder.Build();

            /*var resourceManager = ActivatorUtilities.CreateInstance<ResourceManager>(host.Services);
            var mapInstance = new MapInstance(resourceManager, 100010100);*/
            /*var mapRenderer = new MapRenderer(resourceManager);
            var map = resourceManager.GetMap(100000000);// 100000000);// 100010100);*/
            //var carrots = map.Layers.SelectMany(layer => layer.Objects.Select(obj => new { Layer = layer.Id, obj })).Where(x => x.obj.GroupName == "acc1" && x.obj.Name == "grassySoil" && x.obj.SubsetName == "nature" && x.obj.PartId == "8").ToList();
            /*var width = 2048 * 4;
            var height = 2048;
            var bitmap = new SKBitmap(width, height);
            var canvas = new SKCanvas(bitmap);
            mapInstance.Draw(canvas, 0, 0);
            //var bitmap = mapRenderer.Render(map, width, height, 256 * 5, height / 2);
            using var stream = File.OpenWrite("render.png");
            bitmap.Encode(stream, SkiaSharp.SKEncodedImageFormat.Png, 100);*/

            var window = ActivatorUtilities.CreateInstance<Game>(host.Services);
            window.Run();
            return;
        }
    }
}