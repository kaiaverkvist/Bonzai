using System;
using System.Collections.Generic;
using System.Linq;
using Bonzai.Config;
using Bonzai.Networking;
using Bonzai.Routing;
using Fleck;

namespace Bonzai
{
    /// <summary>
    /// Contains the entire Bonzai server application.
    /// This is what you should initialize in your implementation.
    /// </summary>
    public class BonzaiServer
    {
        /// <summary>
        /// Contains the Fleck instance we're using to handle websocket communications.
        /// Gets set in constructor.
        /// </summary>
        private readonly WebSocketServer _webSocketServer;

        /// <summary>
        /// Holds an instance of the Bonzai router which responsible for handling all incoming network messages. 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public Router Router;

        public List<IWebSocketConnection> Sockets = new List<IWebSocketConnection>();
        
        /// <summary>
        /// Creates an instance of the Bonzai server.
        /// </summary>
        public BonzaiServer(ServerOptions options)
        {
            Router = new Router();
            
            _webSocketServer = new WebSocketServer(options.GetWebsocketUrl());
            
            // If we're running on the SSL scheme and our certificate isn't null we can set the websocket certificate.
            if (options.Scheme == NetworkScheme.SslScheme && options.GetCertificate() != null)
            {
                _webSocketServer.Certificate = options.GetCertificate();
            }
        }

        /// <summary>
        /// Starts Bonzai.
        /// </summary>
        public void Start()
        {
            _webSocketServer.Start((socket) =>
            {
                socket.OnOpen = () =>
                {
                    Sockets.Add(socket);
                    Router.Trigger(socket, new OnConnected());
                };
                socket.OnClose = () =>
                {
                    Sockets.Remove(socket);
                    Router.Trigger(socket, new OnDisconnected("closed"));
                };
                socket.OnMessage = data => Router.ParseAndTrigger(socket, data);
            });
        }

        /// <summary>
        /// Returns the actual websocket server instance held by Bonzai.
        /// </summary>
        /// <returns></returns>
        public WebSocketServer GetWebsocketServer()
        {
            return _webSocketServer;
        }

        /// <summary>
        /// Sends a string message to a specific socket identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        public void SendMessageToId(Guid identifier, string message)
        {
            var socket = Sockets.FirstOrDefault(s => s.ConnectionInfo.Id == identifier);
            socket?.Send(message);
        }
        
        /// <summary>
        /// Sends a byte array message to a specific socket identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        public void SendMessageToId(Guid identifier, byte[] message)
        {
            var socket = Sockets.FirstOrDefault(s => s.ConnectionInfo.Id == identifier);
            socket?.Send(message);
        }

        /// <summary>
        /// Sends a string message to all clients.
        /// </summary>
        /// <param name="message"></param>
        public void SendToAll(string message)
        {
            Sockets.ForEach(s => s.Send(message));
        } 
        
        /// <summary>
        /// Sends a byte array message to all clients.
        /// </summary>
        /// <param name="message"></param>
        public void SendToAll(byte[] message)
        {
            Sockets.ForEach(s => s.Send(message));
        }
    }
}