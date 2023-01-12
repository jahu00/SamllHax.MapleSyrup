// See https://aka.ms/new-console-template for more information
using SkiaSharp;

Console.WriteLine("Hello, World!");
var bitmap = new SKBitmap(320, 240);
var canvas = new SKCanvas(bitmap);
var slime = SKBitmap.Decode("slime.png");

var slimeMatrix1 = SKMatrix.CreateScale(2, 2);
var slimeMatrix2 = SKMatrix.CreateScale(-1, 1).PostConcat(SKMatrix.CreateTranslation(slime.Width, 0));
canvas.SetMatrix(slimeMatrix1);
canvas.DrawBitmap(slime, 0, 0);
canvas.SetMatrix(slimeMatrix2);
canvas.DrawBitmap(slime, 0, 0);
using var stream = File.OpenWrite("output.png");
bitmap.Encode(stream, SkiaSharp.SKEncodedImageFormat.Png, 100);
