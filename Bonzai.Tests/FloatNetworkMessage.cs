using Bonzai.Networking;

namespace Bonzai.Tests
{
    public class FloatNetworkMessage : BaseNetworkMessage
    {
        public float X { get; set; }
            
        public FloatNetworkMessage(float x) : base(null)
        {
            this.X = x;
        }
    }
}