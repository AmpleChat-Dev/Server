using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace AmpleChatServer.Services {
    public abstract class ApiService  : WebSocketBehavior {

        public string ApiController { get; set; }

        public HttpClient HttpClient { get; set; }

        public async Task<HttpResponseMessage> Get(string parameters = "") {
            try {
                return await HttpClient.GetAsync($@"{ApiController}/{(parameters.Length > 0 ? parameters : string.Empty)}");
            }
            catch (Exception e) {
                return new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.BadGateway,
                    Content = new StringContent(e.Message)
                };
            }
        }

        public async Task<HttpResponseMessage> Post(string json = "", string parameters = "") {
            try {
                return await HttpClient.PostAsync($@"{ApiController}/{(parameters.Length > 0 ? parameters : string.Empty)}", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e) {
                return new HttpResponseMessage{
                    StatusCode = System.Net.HttpStatusCode.BadGateway,
                    Content = new StringContent(e.Message)
                };
            }
        }

        public async Task<HttpResponseMessage> Put(string json = "", string parameters = "") {
            try {
                return await HttpClient.PutAsync($@"{ApiController}/{(parameters.Length > 0 ? parameters : string.Empty)}", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e) {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadGateway,
                    Content = new StringContent(e.Message)
                };
            }
        }

    }
}
