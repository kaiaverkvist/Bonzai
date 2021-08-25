using Fleck;

namespace Bonzai.Networking
{
    public class OnConnected : BaseNetworkMessage
    {
        public OnConnected(IWebSocketConnection sender) : base(sender)
        {
        }
    }
}