using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SixLabors.ImageSharp.Drawing.Processing;

namespace Tiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandline = args.
                Select((x, i) => new { index = i, option = x }).
                Where(x => x.option.StartsWith("-")).
                Select(x => new
                {
                    option = x.option[1..],
                    arguments = args.
                                Skip(x.index + 1).
                                TakeWhile(m => !m.StartsWith("-")).ToArray()
                }).
                ToDictionary(x => x.option.ToLower(), x => x.arguments);

            if (args.Length == 0 || commandline.ContainsKey("h") || commandline.ContainsKey("help"))
            {
                Console.Write("Tiler XYZ tile maker v1.0\n" +
                                  "Usage:\n" +
                                  "Tiler.exe image.jpg\n" +
                                  " or\n" +
                                  $"Tiler.exe -input image.jpg -size 256 -zoom 6 -filename \"image.jpg-tiles{Path.DirectorySeparatorChar}{{z}}_{{x}}_{{y}}.png\"\n");
                return;
            }

            var sourceFile = commandline.Count == 0 && args.Length > 0 ? args[0] : commandline["input"].First();
            int tile_size = commandline.ContainsKey("size") ? int.Parse(commandline["size"].First()) : 256;
            using var OriginalSource = commandline.ContainsKey("stdin") ? Image.Load(Console.OpenStandardInput()) : Image.Load(sourceFile);

            int maxzoom = commandline.ContainsKey("zoom") ? int.Parse(commandline["zoom"].First()) :
                ((int)Math.Ceiling(Math.Log(Math.Max(OriginalSource.Width, OriginalSource.Height) / (double)tile_size)) + 1); // auto detect max level

            for (int zoom = 0; zoom <= maxzoom; zoom++)
            {
                int tiles = (int)Math.Pow(2.0, zoom);
                int size = tiles * tile_size;
                using var Scaled = new Image<Rgba32>(size, size);
                Scaled.Mutate(i => i.Clear(Color.Transparent)); // Clear the background to transparent

                int width, height;

                if (OriginalSource.Width > OriginalSource.Height) // Calculate the right scale ratio
                    (width, height) = (Scaled.Width, Scaled.Width * OriginalSource.Height / OriginalSource.Width);
                else
                    (height, width) = (Scaled.Height, Scaled.Height * OriginalSource.Width / OriginalSource.Height);


                var Sampler = // Use Lanczos3 for reducing, Bicubic for upscaling
                    width < OriginalSource.Width || height < OriginalSource.Height
                    ? (IResampler)new LanczosResampler(3.0f)
                    : (IResampler)new BicubicResampler();

                // Scale the entire image
                using (Image<Rgba32> Rescaled = new Image<Rgba32>(OriginalSource.Width, OriginalSource.Height))
                {
                    Rescaled.Mutate(x => x.DrawImage(OriginalSource, new Point(0, 0), 1.0f).Resize(width, height, Sampler));
                    Scaled.Mutate(x => x.DrawImage(Rescaled, new Point((Scaled.Width - width) / 2, (Scaled.Height - height) / 2), new GraphicsOptions()));
                }

                // Take tiles from it
                Enumerable.Range(0, tiles).SelectMany(x => Enumerable.Range(0, tiles).Select(y => new { x, y })).AsParallel().ForAll((c) =>
                {
                    using var Tile = new Image<Rgba32>(tile_size, tile_size);
                    lock (Scaled)
                        Tile.Mutate(x => x.DrawImage(Scaled.Clone(x => x.Crop(new Rectangle(c.x * tile_size, c.y * tile_size, tile_size, tile_size))), new Point(0, 0), new GraphicsOptions()));

                    var Variables = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase) { { "z", zoom }, { "x", c.x }, { "y", c.y } };

                    var Filename = Regex.Replace(
                        commandline.ContainsKey("filename") && commandline["filename"].Length > 0
                        ? commandline["filename"].First()
                        : $"{sourceFile}-tiles{Path.DirectorySeparatorChar}{{z}}_{{x}}_{{y}}.png",
                        @"{(?<var>[^}]+)}", new MatchEvaluator(m => $"{Variables[m.Groups["var"].Value]}"));

                    var OutputDirectory = Path.GetDirectoryName(Filename);

                    if (OutputDirectory != null && !Directory.Exists(OutputDirectory))
                        Directory.CreateDirectory(OutputDirectory);

                    Tile.Save(Filename);
                });
            }
        }
    }
}
