using WebSocketSharp.Server;
using System;

namespace AmpleChatServer
{
    class Program
    {
        static readonly int PORT_NUM = 49152;
        static readonly string URL = $"ws://localhost:{PORT_NUM}";
        static readonly string EXIT_COMMAND = "exit";

        static void Main(string[] args)
        {
            var ws = new WebSocketServer(URL);

            ws.AddWebSocketService("/do_you_know_the_way", () => new WayPoint { });
            
            ws.Start();

            Console.WriteLine($"Server running on : {URL}");

            while (Console.ReadLine() != EXIT_COMMAND);

            ws.Stop();

            Console.WriteLine("Server stopped, system exit");
        }
    }
}
