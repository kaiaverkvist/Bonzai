using Bonzai.Networking;
using Fleck;

namespace Bonzai.ExampleServer
{
    public class ChatMessage : BaseNetworkMessage
    {
        public string Text { get; set; }
        
        public ChatMessage(IWebSocketConnection sender, string text) : base(sender)
        {
            this.Text = text;
        }
    }
}