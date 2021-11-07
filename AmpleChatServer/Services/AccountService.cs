using AmpleChatLibrary.User;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AmpleChatServer.Services {
    public class AccountService : WebSocketBehavior {
        const int REGISTER = 0;
        const int LOGIN = 1;
        static readonly string USER_API_URL_DEV = "http://localhost:44396/api/user";

        HttpClient httpClient;

        public AccountService(HttpClient cliet) {

            httpClient = cliet;
        }

        protected override void OnOpen() {
            base.OnOpen();
            Send(Guid.NewGuid().ToString());
        }

        protected async override void OnMessage(MessageEventArgs e) {

            // "0,name,password"
            var query = e.Data.Split(",");
            int type;

            var hasValue = int.TryParse(query[0], out type);

            if (hasValue) {

                switch(type) {
                    case REGISTER:
                        {
                            var model = new RegisterModel {
                                Email = query[1],
                                Password = query[2],
                                UserName = query[3]
                            };

                            StringContent data = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                            try {

                                var response = await httpClient.PutAsync("account", data);

                                if (response.StatusCode == 0) {
                                    Send("Regiser worksk");
                                }
                                else {
                                    Send(JsonSerializer.Serialize(response));
                                }
                            }
                            catch (Exception ex) {
                                Send(JsonSerializer.Serialize(ex));
                            }


                        }
                        break;

 

                    case LOGIN:
                       {
                            var model = new LoginModel {
                                UserNameOrEmail = query[1],
                                Password = query[2]
                            };

                            StringContent data = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                            try {

                                var response = await httpClient.PostAsync("account", data);

                                if (response.StatusCode == 0) {
                                    Send("Login works");
                                }
                                else {
                                    Send(JsonSerializer.Serialize(response));
                                }
                            }
                            catch (Exception ex) {
                                Send(JsonSerializer.Serialize(ex));
                            }

                        }
                        break;

                    default:break;
                }

            }
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e) {
            base.OnError(e);
        }
    }
}
