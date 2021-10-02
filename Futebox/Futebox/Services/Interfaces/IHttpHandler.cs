using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IHttpHandler
    {
        HttpResponseMessage Get(string url, Dictionary<string, string> headers = null);
        HttpResponseMessage Post(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}
