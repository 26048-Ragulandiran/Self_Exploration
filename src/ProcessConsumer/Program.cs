using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ProcessConsumer
{
    class Consumer
    {
        static async Task Main()
        {
            TcpClient producerClient = new TcpClient();
            await producerClient.ConnectAsync("127.0.0.1", 5000);

            TcpClient uiClient = new TcpClient();
            await uiClient.ConnectAsync("127.0.0.1", 6000);

            Console.WriteLine($"Consumer {Environment.ProcessId} started");

            var reader = new StreamReader(producerClient.GetStream());

            while (true)
            {
                var line = await reader.ReadLineAsync();

                if (line == null) continue;

                var message = JsonSerializer.Deserialize<Message>(line);

                var processed = Process(message);

                string json = JsonSerializer.Serialize(processed) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);

                await uiClient.GetStream().WriteAsync(data);

                Console.WriteLine($"Processed: {processed.Data}");
            }
        }

        static Message Process(Message msg)
        {
            return new Message
            {
                Id = msg.Id,
                Data = $"{msg.Data} → Processed by Consumer {Environment.ProcessId}"
            };
        }
    }
}