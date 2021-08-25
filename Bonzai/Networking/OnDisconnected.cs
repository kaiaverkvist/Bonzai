using Fleck;

namespace Bonzai.Networking
{
    public class OnDisconnected : INetworkMessage
    {
        public string Message { get; }
        
        public OnDisconnected(string message = "bye")
        {
            this.Message = message;
        }
    }
}