using Bonzai.Networking;

namespace Bonzai.Tests
{
    public class TestNetworkMessage : INetworkMessage
    {
        public string Identifier { get; set; }

        public TestNetworkMessage(string identifier)
        {
            Identifier = identifier;
        }
    }
}