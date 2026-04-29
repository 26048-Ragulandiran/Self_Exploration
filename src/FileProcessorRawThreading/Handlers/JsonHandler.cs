using System.Text.Json;

namespace FileProcessorRawThreading.Handlers
{
    public static class JsonHandler
    {
        public static void Process(List<string> files, string outputBase)
        {
            foreach (var file in files)
            {
                string outputDir = Path.Combine(outputBase, "Json");
                Directory.CreateDirectory(outputDir);

                string name = Path.GetFileNameWithoutExtension(file);
                string output = Path.Combine(outputDir, name + "_processed.json");

                var content = File.ReadAllText(file);

                var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(content)
                           ?? new();

                foreach (var item in list)
                    item["processed"] = true;

                File.WriteAllText(output,
                    JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));

                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: [json] processed {file}");
            }
        }
    }
}