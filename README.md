# Bonzai
    Bonzai is a library that aims to make writing web socket servers really simple.

## Goals
* To be able to define handlers for websocket messages based on their actual signature, rather than a predefined string.
* Good developer UX.

## Basic usage guide:
This is basically all you need for a basic server. See https://github.com/kaiaverkvist/Bonzai/tree/main/Bonzai.ExampleServer/ for the full example code.
```cs
class Program
{
    private static BonzaiServer _server;

    static void Main(string[] args)
    {

            Console.WriteLine("Starting bonzai server...");
            Console.WriteLine($"Adding {typeof(ChatMessage).FullName}");

            _server = new BonzaiServer(new ServerOptions("0.0.0.0", 8085, NetworkScheme.NoSslScheme));

            // Handle a chat message.
            _server.Router.Register<ChatMessage>((sender, message) =>
            {
                Console.WriteLine($"Recevied chat message with text: {message.Text}");
            });

            // The OnConnected and OnDisconnected messages are special messages sent by Bonzai.
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
```

## Contributions
Any contributions are welcome.

## Builds and Tests
[![Build and Test](https://github.com/kaiaverkvist/Bonzai/actions/workflows/dotnet.yml/badge.svg)](https://github.com/kaiaverkvist/Bonzai/actions/workflows/dotnet.yml)
