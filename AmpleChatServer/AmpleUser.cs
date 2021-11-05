using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AmpleChatServer
{
    public class AmpleUser : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Console.WriteLine($"New connection: {ID}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            
        }

        protected override void OnClose(CloseEventArgs e)
        {

        }

        protected override void OnError(ErrorEventArgs e)
        {

        }
    }
}
