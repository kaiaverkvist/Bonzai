using Fleck;

namespace Bonzai.Networking
{
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