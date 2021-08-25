using Bonzai.Networking;
using Fleck;

namespace Bonzai.ExampleServer
{
    public class ChatMessage : INetworkMessage
    {
        public string Text { get; set; }
        
        public ChatMessage(string text)
        {
            this.Text = text;
        }
    }
}