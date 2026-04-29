namespace FileProcessorRawThreading.Handlers
{
    public static class CsvHandler
    {
        public static void Process(List<string> files, string outputBase)
        {
            foreach (var file in files)
            {
                string outputDir = Path.Combine(outputBase, "Csv");
                Directory.CreateDirectory(outputDir);

                string name = Path.GetFileNameWithoutExtension(file);
                string output = Path.Combine(outputDir, name + "_processed.csv");

                var lines = File.ReadAllLines(file)
                                .Select(l => l + ",processed");

                File.WriteAllLines(output, lines);

                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: [csv] processed {file}");
            }
        }
    }
}