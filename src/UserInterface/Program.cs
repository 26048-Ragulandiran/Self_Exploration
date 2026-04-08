using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace UserInterface
{
    class UIApp
    {
        static async Task Main()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 6000);
            listener.Start();

            Console.WriteLine("UI listening on port 6000...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClient(client);
            }
        }

        static async Task HandleClient(TcpClient client)
        {
            var reader = new StreamReader(client.GetStream());

            while (true)
            {
                var line = await reader.ReadLineAsync();

                if (line == null) break;

                var message = JsonSerializer.Deserialize<Message>(line);

                Console.WriteLine($"[UI] {message.Data}");
            }
        }
    }
}