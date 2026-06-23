using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace NavigationApp
{
    public class ServerClient
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiToken = "lip_3Y3lFqjXXQd6xCIzRz7X";
        private uint rating { get; }

        public delegate void Handler(JObject jsonObj);
        public event Handler GameStartEvent;

        public delegate void Status();
        public event Status Error_ReadStreamIncomingEvents;


        public async Task<bool> CreateSeek(bool rated, int time, uint increment, string color)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, "https://lichess.org/api/board/seek"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
                    var formParams = new Dictionary<string, string>
                    {
                        ["rated"] = rated ? "true" : "false",
                        ["variant"] = "standard",
                        ["ratingRange"] = $"{rating - 200}-{rating + 200}",
                        ["time"] = time.ToString(),
                        ["increment"] = increment.ToString(),
                        ["color"] = color,
                    };
                    request.Content = new FormUrlEncodedContent(formParams);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch { return false; }
        }

        public async Task<JObject> GetAccount()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://lichess.org/api/account"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    JObject jsonObj = JObject.Parse(await response.Content.ReadAsStringAsync());
                    return jsonObj;
                }
            }
        }

        public async Task ReadStreamIncomingEvents()
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, "https://lichess.org/api/stream/event"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

                    using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            Error_ReadStreamIncomingEvents?.Invoke();
                            return;
                        }

                        using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            JObject jsonObj;
                            string line;
                            while (true)
                            {
                                line = await sr.ReadLineAsync().ConfigureAwait(false);
                                if (line != "" && line != null && line.Length > 1)
                                {
                                    jsonObj = JObject.Parse(line);
                                    {
                                        if ((string)jsonObj["type"] == "gameStart")
                                        {
                                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                                                Windows.UI.Core.CoreDispatcherPriority.Normal,
                                                () => GameStartEvent?.Invoke(jsonObj)
                                            );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Error_ReadStreamIncomingEvents?.Invoke();
            }
        }
    }
}
