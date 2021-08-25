using Bonzai.Networking;

namespace Bonzai.Tests
{
    public class FloatNetworkMessage : INetworkMessage
    {
        public float X { get; set; }
            
        public FloatNetworkMessage(float x)
        {
            this.X = x;
        }
    }
}