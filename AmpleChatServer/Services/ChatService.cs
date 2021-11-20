using AmpleChatServer.Models;
using AmpleChatServer.Services.Packets;
using Newtonsoft.Json;
using System;
using WebSocketSharp;
using static AmpleChatServer.Services.Packets.Packet;

namespace AmpleChatServer.Services {
    public class ChatService : ApiService {

        protected override void OnOpen() {
            base.OnOpen();
            SendPacket(new Packet(PacketType.INFO_PACKET).Add("connectionId", ID));
        }

        protected async override void OnMessage(MessageEventArgs e) {

            var message = e.Data;

            if (string.IsNullOrEmpty(message)) {
                SendPacket(new ErrorPacket()
                    .Message("Empty message"));
                Sessions.CloseSession(ID);
                return;
            }

            var packet = ParsePacket(message);

            Console.WriteLine(packet);
        }


        private void SendPacket(Packet packet) {
            Send(packet.ToString());
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e) {
            base.OnError(e);
            Console.WriteLine($"[ERROR]: {e.Message}");
        }

    }
}
