using Fleck;

namespace Bonzai.Networking
{
    public class OnDisconnected : BaseNetworkMessage
    {
        public string Message { get; }
        
        public OnDisconnected(IWebSocketConnection sender, string message = "bye") : base(sender)
        {
            this.Message = message;
        }
    }
}