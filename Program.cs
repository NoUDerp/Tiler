using System;
using System.Linq;

namespace Tiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandline = args.Select((x, i) => new { index = i, option = x }).Where(x => x.option.StartsWith("-")).Select(x => new { option = x.option.Substring(1), arguments = args.Skip(x.index).TakeWhile(m => !m.StartsWith("-")).ToArray() }).ToDictionary(x => x.option.ToLower(), x => x.arguments);
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
            var Source = commandline.ContainsKey("stdin") ? System.Drawing.Bitmap.FromStream(System.Console.OpenStandardInput()) : System.Drawing.Bitmap.FromFile(sourceFile);
            int maxzoom = commandline.ContainsKey("zoom") ? int.Parse(commandline["zoom"].First()) :
                ((int)Math.Ceiling(Math.Log(Math.Max(Source.Width, Source.Height) / (double)tile_size)) + 1); // auto detect max level
            for (int zoom = 0; zoom <= maxzoom; zoom++)
            {
                int tiles = (int)Math.Pow(2.0, zoom);
                int size = tiles * tile_size;
                using (var Scaled = new System.Drawing.Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (var g = System.Drawing.Graphics.FromImage(Scaled))
                    {
                        g.Clear(System.Drawing.Color.Transparent);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        if (Source.Width > Source.Height)
                        {
                            int width = Scaled.Width;
                            int height = width * Source.Height / Source.Width;
                            g.DrawImage(Source, new System.Drawing.RectangleF(0.0f, (Scaled.Height - height) / 2.0f, width, height), new System.Drawing.RectangleF(0.0f, 0.0f, Source.Width, Source.Height), System.Drawing.GraphicsUnit.Pixel);
                        }
                        else
                        {
                            int height = Scaled.Height;
                            int width = height * Source.Width / Source.Height;
                            g.DrawImage(Source, new System.Drawing.RectangleF((Scaled.Width - width) / 2.0f, 0.0f, width, height), new System.Drawing.RectangleF(0, 0, Source.Width, Source.Height), System.Drawing.GraphicsUnit.Pixel);

                        }
                    }
                    Enumerable.Range(0, tiles).SelectMany(x => Enumerable.Range(0, tiles).Select(y => new { x, y })).AsParallel().ForAll((c) =>
                    {
                        using (var Tile = new System.Drawing.Bitmap(tile_size, tile_size, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                        using (var g = System.Drawing.Graphics.FromImage(Tile))
                        {
                            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                            lock (Source)
                                g.DrawImage(Scaled, new System.Drawing.Rectangle(0, 0, tile_size, tile_size), c.x * tile_size, c.y * tile_size, tile_size, tile_size, System.Drawing.GraphicsUnit.Pixel);
                            var l = OutputDirectory.FullName + System.IO.Path.DirectorySeparatorChar + zoom.ToString() + "_" + c.x.ToString() + "_" + c.y.ToString() + ".png";
                            Tile.Save(l);
                        }
                    });
                }
            }
        }
    }
}
