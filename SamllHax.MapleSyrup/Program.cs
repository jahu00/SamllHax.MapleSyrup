using SamllHax.MapleSyrup.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SamllHax.MapleSyrup.Interfaces.Providers;
using SamllHax.MapleSyrup.Providers.Dumper;

namespace SamllHax.MapleSyrup
{
    public static class Program
    {
        public static void Main()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IResourceProvider, DumperResourceProvider>();
                    services.AddSingleton<ResourceManager>();
                    services.AddSingleton<MapRenderer>();
                    services.AddSingleton<Window>();
                }).Build();

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

            var window = ActivatorUtilities.CreateInstance<Window>(host.Services);
            window.Run();
            return;
        }
    }
}