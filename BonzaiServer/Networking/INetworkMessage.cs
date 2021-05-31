using Fleck;

namespace Bonzai.Networking
{
    public interface INetworkMessage
    {
        IWebSocketConnection Sender();
    }
}