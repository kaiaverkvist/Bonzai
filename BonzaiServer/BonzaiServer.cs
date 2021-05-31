﻿using Bonzai.Config;
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
        public static Router Router; 
        
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
            
            _webSocketServer.Start((socket) =>
            {
                socket.OnOpen = () => Router.Trigger(new OnConnected(socket));
                socket.OnClose = () => Router.Trigger(new OnDisconnected(socket, "quit"));
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
    }
}