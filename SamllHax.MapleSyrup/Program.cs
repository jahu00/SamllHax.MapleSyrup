using SamllHax.MapleSyrup.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var resourceManager = ActivatorUtilities.CreateInstance<ResourceManager>(host.Services);

            /*using var client = new JurneyClient();
            client.Loop();*/
            //var resourceManager = new ResourceManager(@"D:\WinXP_Share\MS83 dump");
            var mapRenderer = new MapRenderer(resourceManager);
            var map = resourceManager.GetMap(100000000);// 100000000);// 100010100);
            var carrots = map.Layers.SelectMany(layer => layer.Objects.Select(obj => new { Layer = layer.Id, obj })).Where(x => x.obj.GroupName == "acc1" && x.obj.Name == "grassySoil" && x.obj.SubsetName == "nature" && x.obj.PartId == "8").ToList();
            var width = 2048 * 4;
            var height = 2048;
            var bitmap = mapRenderer.Render(map, width, height, 256 * 5/*width / 2*/, height / 2);
            using var stream = File.OpenWrite("render.png");
            bitmap.Encode(stream, SkiaSharp.SKEncodedImageFormat.Png, 100);
            /*var window = new Window();
            window.Run();*/
            return;
        }
    }
}