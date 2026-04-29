namespace FileProcessorRawThreading.Handlers
{
    public static class TxtHandler
    {
        public static void Process(List<string> files, string outputBase)
        {
            foreach (var file in files)
            {
                string outputDir = Path.Combine(outputBase, "Txt");
                Directory.CreateDirectory(outputDir);

                string name = Path.GetFileNameWithoutExtension(file);
                string output = Path.Combine(outputDir, name + "_processed.txt");

                var text = File.ReadAllText(file) + "\n[Processed]";
                File.WriteAllText(output, text);

                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: [txt] processed {file}");
            }
        }
    }
}