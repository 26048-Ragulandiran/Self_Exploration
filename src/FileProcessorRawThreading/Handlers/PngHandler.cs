using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FileProcessorRawThreading.Handlers
{
    public static class PngHandler
    {
        public static void Process(List<string> files, string outputBase)
        {
            foreach (var file in files)
            {
                string outputDir = Path.Combine(outputBase, "Png");
                Directory.CreateDirectory(outputDir);

                string name = Path.GetFileNameWithoutExtension(file);
                string output = Path.Combine(outputDir, name + "_processed.png");

                using var image = Image.Load(file);
                image.Mutate(x => x.Rotate(90));
                image.SaveAsPng(output);

                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: [png] processed {file}");
            }
        }
    }
}