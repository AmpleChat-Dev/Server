using WebSocketSharp.Server;
using System;
using AmpleChatServer.Services;
//using System.Net;
//using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace AmpleChatServer {
    class Program {
        static readonly int PORT_NUM = 49152;
        static readonly string URL = $"ws://localhost:{PORT_NUM}";
        static readonly string EXIT_COMMAND = "exit";
        static readonly string API_URL = "http://localhost:44396/api/";

        static void Main(string[] args) {
            // https://www.thecodebuzz.com/using-httpclientfactory-in-net-core-console-application/
            //var builder = new HostBuilder()
            //    .ConfigureServices((hostContext, services) =>
            //    {

            //    }).UseConsoleLifetime();

            var socketsHandler = new SocketsHttpHandler {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 20
            };

            var client = new HttpClient(socketsHandler) {
                BaseAddress = new Uri(API_URL)
            };

            var ws = new WebSocketServer(URL);

            ws.AddWebSocketService("/account", () => new AccountService {
                ApiController = "account",
                HttpClient = client
            });

            ws.AddWebSocketService("/chat", () => new ChatService {
                ApiController = "chat",
                HttpClient = client
            });

            ws.Start();

            Console.WriteLine($"Server running on : {URL}");

            var cmd = string.Empty;
            var read = true;

            while (read) 
            {
                if (cmd == EXIT_COMMAND) // I0 ????
                    read = false;


                cmd = Console.ReadLine();
            }

            ws.Stop();

            Console.WriteLine("System exit");
        }
    }
}
