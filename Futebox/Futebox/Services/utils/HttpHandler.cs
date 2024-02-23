using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class HttpHandler : IHttpHandler
    {
        private HttpClient _client = new HttpClient();

        public HttpHandler()
        {

        }

        public HttpHandler SetTimeoutMinutes(int minutes)
        {
            _client.Timeout = TimeSpan.FromMinutes(minutes);
            return this;
        }

        public HttpResponseMessage Get(string url, Dictionary<string, string> headers = null)
        {
            return GetAsync(url, headers).Result;
        }

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            return PostAsync(url, content).Result;
        }

        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            using (var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, url))
            {
                headers?.Keys.ToList().ForEach(_ =>
                    requestMessage.Headers.Add(_, headers[_])
                );

                return await _client.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _client.PostAsync(url, content);
        }
    }
}
