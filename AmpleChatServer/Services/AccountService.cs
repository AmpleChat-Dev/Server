using System;
using System.Net;
using System.Threading.Tasks;
using WebSocketSharp;
using AmpleChatServer.Services.Packets;
using AmpleChatServer.Models;
using static AmpleChatServer.Services.Packets.Packet;
using Newtonsoft.Json;

namespace AmpleChatServer.Services {
    public class AccountService : ApiService {

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

            string json;
            var id = packet.GetPacketId();

            switch (id) {
                case PacketType.REGISER_PACKET:

                    json = JsonConvert.SerializeObject(new RegisterModel
                    {
                        Email = packet.Get("email"),
                        Password = packet.Get("password"),
                        UserName = packet.Get("username")
                    });

                    await HandleRegister(json);

                    break;

                case PacketType.LOGIN_PACKET:

                    json = JsonConvert.SerializeObject(new LoginModel
                    {
                        Password = packet.Get("password"),
                        UserNameOrEmail = packet.Get("credentials"),
                    });

                    await HandleLogin(json);

                    break;

                default:
                    Sessions.CloseSession(ID);
                    break;
            }
        }

        private async Task HandleRegister(string json) {
            var putResult = await Put(json);

            switch (putResult.StatusCode) {

                case HttpStatusCode.OK:
                    SendPacket(new Packet(PacketType.REGISER_RESPONSE)
                        .Add("accountCreated", true));
                    break;

                case HttpStatusCode.BadRequest:
                case HttpStatusCode.BadGateway:
                    SendPacket(new ErrorPacket()
                        .Message(await putResult.Content.ReadAsStringAsync()));
                    break;

                default:
                    SendPacket(new ErrorPacket()
                        .Message("Unkown error"));
                    break;
            }
        }

        private async Task HandleLogin(string json) {

            var postResult = await Post(json);

            switch (postResult.StatusCode) {

                case HttpStatusCode.OK:
                    SendPacket(new Packet(PacketType.LOGIN_RESPONSE)
                        .Add("accountAuthenticated", true));
                    break;

                case HttpStatusCode.BadRequest:
                case HttpStatusCode.BadGateway:
                    SendPacket(new ErrorPacket()
                        .Message(await postResult.Content.ReadAsStringAsync()));
                    break;
            }
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
