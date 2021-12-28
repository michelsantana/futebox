using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Futebox.Services
{
    public class NotifyService : INotifyService
    {
        // Url para obter o chatId
        // https://api.telegram.org/${botToken}/getUpdates
        string botToken => Settings.TelegramBotToken;
        string chatId => Settings.TelegramNotifyUserId;
        string baseUrl => "https://api.telegram.org/" + botToken;
        int messageSize = 1024;

        private static HttpClient _http;
        public Task Notify(string message)
        {
            if (Settings.DEBUGMODE) return Task.FromResult(0);
            _http = _http ?? new HttpClient();
            if (message.Length > messageSize)
                message = message.Substring(0, messageSize);
            message = HttpUtility.UrlEncode(message);
            var result = _http.GetAsync($"{baseUrl}/sendMessage?chat_id={chatId}&text={message}").Result;
            Console.WriteLine(result);
            return Task.FromResult(result);
        }
    }
}
