using System;
using Bonzai.Config;

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
            _server.Router.Register<ChatMessage>(c =>
            {
                Console.WriteLine($"Recevied chat message with text: {c.Text}");
            });
            _server.Start();

            while (Console.ReadLine() != "exit");
        }
    }
}