using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ProducerApp
{
    public class Producer
    {
        private static List<TcpClient> consumers = new();

        static async Task Main()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            Console.WriteLine("Producer started on port 5000...");

            _ = AcceptConsumers(listener);
            _ = ProduceData();

            Console.ReadLine();
        }

        static async Task AcceptConsumers(TcpListener listener)
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                consumers.Add(client);
                Console.WriteLine("Consumer connected");
            }
        }

        static async Task ProduceData()
        {
            HttpClient client = new HttpClient();

            while (true)
            {
                try
                {
                    string url = "https://jsonplaceholder.typicode.com/posts/1";

                    var response = await client.GetStringAsync(url);

                    var message = new Message
                    {
                        Id = Guid.NewGuid().ToString(),
                        Data = ExtractTitle(response)
                    };

                    string json = JsonSerializer.Serialize(message);
                    byte[] data = Encoding.UTF8.GetBytes(json + "\n");

                    foreach (var consumer in consumers.ToList())
                    {
                        try
                        {
                            await consumer.GetStream().WriteAsync(data);
                        }
                        catch
                        {
                            consumers.Remove(consumer);
                        }
                    }

                    Console.WriteLine($"Produced: {message.Data}");

                    await Task.Delay(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static string ExtractTitle(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("title").GetString();
        }
    }

}