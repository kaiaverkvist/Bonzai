using Fleck;

namespace Bonzai.Networking
{
    /// <summary>
    /// The Base Network Message is a network message that contains a field indicating
    /// the websocket Sender of the message.
    /// </summary>
    public class BaseNetworkMessage : INetworkMessage
    {
        private IWebSocketConnection _sender { get; }
        
        public BaseNetworkMessage(IWebSocketConnection sender)
        {
            this._sender = sender;
        }

        public IWebSocketConnection Sender()
        {
            return _sender;
        }
    }
}