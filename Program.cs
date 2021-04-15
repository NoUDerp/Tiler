using System;
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
            var commandline =
                args.Select((x, i) => new { index = i, option = x })
                .Where(x => x.option.StartsWith("-")).Select(x => new
                {
                    option = x.option.Substring(1),
                    arguments = args.Skip(x.index + 1)
                .TakeWhile(m => !m.StartsWith("-")).ToArray()
                })
                .ToDictionary(x => x.option.ToLower(), x => x.arguments);

            if (args.Length == 0 || commandline.ContainsKey("h") || commandline.ContainsKey("help"))
            {
                Console.WriteLine("Tiler XYZ tile maker v1.0");
                Console.WriteLine("Usage:");
                Console.WriteLine("Tiler.exe image.jpg");
                Console.WriteLine(" or");
                Console.WriteLine("Tiler.exe -input image.jpg -output output_folder -size 256 -zoom 6");
                return;
            }

            var sourceFile = commandline.Count == 0 && args.Length > 0 ? args[0] : commandline["input"].First();
            int tile_size = commandline.ContainsKey("size") ? int.Parse(commandline["size"].First()) : 256;
            var Directory = commandline.ContainsKey("output") ? commandline["output"].First() : new System.IO.FileInfo(sourceFile).Directory.FullName + System.IO.Path.DirectorySeparatorChar + string.Format("{0}-tiles", commandline.ContainsKey("stdin") ? "STDIN" : new System.IO.FileInfo(sourceFile).Name);
            System.IO.DirectoryInfo OutputDirectory = System.IO.Directory.CreateDirectory(Directory);
            var OriginalSource = commandline.ContainsKey("stdin") ? Image<Rgba32>.Load(Console.OpenStandardInput()) : Image<Rgba32>.Load(sourceFile);
            int maxzoom = commandline.ContainsKey("zoom") ? int.Parse(commandline["zoom"].First()) :
                ((int)Math.Ceiling(Math.Log(Math.Max(OriginalSource.Width, OriginalSource.Height) / (double)tile_size)) + 1); // auto detect max level

            var Sampler = new BicubicResampler();

            for (int zoom = 0; zoom <= maxzoom; zoom++)
            {
                int tiles = (int)Math.Pow(2.0, zoom);
                int size = tiles * tile_size;
                using (var Scaled = new Image<Rgba32>(size, size))
                {
                    Scaled.Mutate(i => i.Clear(Color.Transparent));
                    {

                        if (OriginalSource.Width > OriginalSource.Height)
                        {
                            int width = Scaled.Width;
                            int height = width * OriginalSource.Height / OriginalSource.Width;
                            Scaled.Mutate(x => x.DrawImage(OriginalSource.Clone(x => x.Resize(width, height, Sampler)), new Point(0, (Scaled.Height - height) / 2), new GraphicsOptions()));
                        }
                        else
                        {
                            int height = Scaled.Height;
                            int width = height * OriginalSource.Width / OriginalSource.Height;
                            Scaled.Mutate(x => x.DrawImage(OriginalSource.Clone(x => x.Resize(width, height, Sampler)), new Point((Scaled.Width - width) / 2, 0), new GraphicsOptions()));
                        }
                    }
                    Enumerable.Range(0, tiles).SelectMany(x => Enumerable.Range(0, tiles).Select(y => new { x, y })).AsParallel().ForAll((c) =>
                    {
                        using var Tile = new Image<Rgba32>(tile_size, tile_size);
                        lock (Scaled)
                            Tile.Mutate(x => x.DrawImage(Scaled.Clone(x => x.Crop(new Rectangle(c.x * tile_size, c.y * tile_size, tile_size, tile_size))), new Point(0, 0), new GraphicsOptions()));
                        var l = OutputDirectory.FullName + System.IO.Path.DirectorySeparatorChar + zoom.ToString() + "_" + c.x.ToString() + "_" + c.y.ToString() + ".png";
                        Tile.Save(l);
                    });
                }
            }
        }
    }
}
