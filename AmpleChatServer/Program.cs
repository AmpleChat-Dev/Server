using WebSocketSharp.Server;

namespace AmpleChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ws = new WebSocketServer(49152);

            ws.Start();
        }
    }
}
