using FileProcessorRawThreading.Handlers;
using System.Diagnostics;

namespace FileProcessorRawThreading
{
    public static class FileProcessor
    {
        private static string inputPath = @"..\..\..\Data";
        private static string outputPath = @"..\..\..\Processed_Data";

        public static void Run()
        {
            var files = Directory.GetFiles(inputPath, "*.*", SearchOption.AllDirectories);

            var threads = new List<Thread>
            {
                new Thread(() => RunWithTimer("json", () => JsonHandler.Process(GetAppropriateFiles(files, ".json"), outputPath))),
                new Thread(() => RunWithTimer("csv", () => CsvHandler.Process(GetAppropriateFiles(files,".csv"), outputPath))),
                new Thread(() => RunWithTimer("txt", () => TxtHandler.Process(GetAppropriateFiles(files, ".txt"), outputPath))),
                new Thread(() => RunWithTimer("png", () => PngHandler.Process(GetAppropriateFiles(files, ".png"), outputPath))),
                new Thread(() => RunWithTimer("txt + png", () => TxtToPngHandler.Process(GetAppropriateFiles(files, ".txt"), GetAppropriateFiles(files, ".png"), outputPath)))
            };

            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());

            Console.WriteLine("All files processed successfully !!!");
        }

        private static List<string> GetAppropriateFiles(string[] files, string type)
        {
            return files.Where(file => Path.GetExtension(file).Equals(type)).ToList();
        }

        public static void RunWithTimer(string name, Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine($"{name} thread started...");

            action();

            sw.Stop();

            Console.WriteLine($"\n--------------------------------------------------------");
            Console.WriteLine($"          {name} thread finished in {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"--------------------------------------------------------\n");
        }
    }
}