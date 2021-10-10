using WebSocketSharp.Server;
using System.Linq;
using WebSocketSharp;
using System;

namespace AmpleChatServer
{
    public class WayPoint : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Send($"id:{ID}");
            Console.WriteLine($"New connection: {ID}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);

            var randomId = Sessions.ActiveIDs.Where(i => i != ID).FirstOrDefault();

            if (string.IsNullOrEmpty(randomId)) return;

            Sessions.SendTo($"msg:{e.Data}", randomId);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            Console.WriteLine($"Lost connection: {ID} | Reason: {e.Reason}");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            Console.WriteLine($"Error: {e.Message} | Exception: {e.Exception.GetType().FullName}");
        }
    }
}
