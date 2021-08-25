using System;
using Bonzai.Config;
using Bonzai.Networking;

namespace Bonzai.ExampleServer
{
    class Program
    {
        private static BonzaiServer _server;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting bonzai server...");
            Console.WriteLine($"Adding {typeof(ChatMessage).FullName}");

            _server = new BonzaiServer(new ServerOptions("0.0.0.0", 8085, NetworkScheme.NoSslScheme));
            _server.Router.Register<ChatMessage>((sender, message) =>
            {
                Console.WriteLine($"Recevied chat message with text: {message.Text}");
            });
            _server.Router.Register<OnConnected>((sender, msg) =>
            {
                Console.WriteLine($"Connection opened with {sender.ConnectionInfo.Id}");
            });
            _server.Router.Register<OnDisconnected>((sender, msg) =>
            {
                Console.WriteLine($"Connection closed by {sender.ConnectionInfo.Id}");
            });
            _server.Start();

            while (Console.ReadLine() != "exit");
        }
    }
}