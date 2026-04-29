using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace FileProcessorRawThreading.Handlers
{
    public static class TxtToPngHandler
    {
        public static void Process(List<string> textFiles, List<string> pngFiles, string outputBase)
        {
            foreach (var textFile in textFiles)
            {
                string name = Path.GetFileNameWithoutExtension(textFile);

                var pngFile = pngFiles.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file).Equals(name));

                if (pngFile == null) continue;

                string outputDir = Path.Combine(outputBase, "Txt_Png");
                Directory.CreateDirectory(outputDir);

                string output = Path.Combine(outputDir, name + "_processed.png");

                string text = File.ReadAllText(textFile);

                using var image = Image.Load(pngFile);

                image.Mutate(ctx =>
                {
                    ctx.DrawText(text,
                        SystemFonts.CreateFont("Arial", 20),
                        Color.Red,
                        new PointF(20, 20));

                    ctx.Rotate(90);
                });

                image.SaveAsPng(output);

                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: [txt + png] processed {name}");
            }
        }
    }
}