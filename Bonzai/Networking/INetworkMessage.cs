using Fleck;

namespace Bonzai.Networking
{
    /// <summary>
    /// The most basic Network Message possible.
    /// Use this if you want to override the BaseNetworkMessage.
    /// </summary>
    public interface INetworkMessage
    {
        IWebSocketConnection Sender();
    }
}