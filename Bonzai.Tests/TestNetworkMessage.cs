using Bonzai.Networking;

namespace Bonzai.Tests
{
    public class TestNetworkMessage : BaseNetworkMessage
    {
        public string Identifier { get; set; }

        public TestNetworkMessage(string identifier) : base(null)
        {
            Identifier = identifier;
        }
    }
}