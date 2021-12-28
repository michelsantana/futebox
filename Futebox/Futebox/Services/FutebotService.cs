using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class FutebotService : IFutebotService
    {
        readonly IHttpHandler _http;

        public FutebotService(IHttpHandler http)
        {
            _http = http;
        }

        public RobotResultApi VerificarConfiguracaoYoutubeBrowser()
        {
            return RoboExecutarRC("ytstatus");
        }
        public RobotResultApi VerificarConfiguracaoInstagramBrowser()
        {
            return RoboExecutarRC("igstatus");
        }

        public RobotResultApi GerarImagem(SubProcesso subProcesso)
        {
            return RoboExecutarRC("imagem", subProcesso);
        }
        public RobotResultApi GerarAudio(SubProcesso subProcesso)
        {
            return RoboExecutarRC("audio", subProcesso);
        }
        public RobotResultApi GerarVideo(SubProcesso subProcesso)
        {
            return RoboExecutarRC("video", subProcesso);
        }
        public RobotResultApi PublicarVideo(SubProcesso subProcesso)
        {
            return RoboExecutarRC("publicar", subProcesso);
        }
        public RobotResultApi AbrirPasta(Processo processo)
        {
            return RoboExecutarRC("pasta", "pasta", Uri.EscapeDataString(Path.GetFullPath(processo.pastaDosArquivos)));
        }

        private RobotResultApi RoboExecutarRC(string command, SubProcesso subProcesso)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(subProcesso);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = _http.Post($"{Settings.RobotEndpointBaseUrl}rc/{command}", data);
            return JsonConvert.DeserializeObject<RobotResultApi>(result.Content.ReadAsStringAsync().Result);
        }

        private RobotResultApi RoboExecutarRC(string command, string queryStringKey, string queryStringValue)
        {
            var result = _http.Get($"{Settings.RobotEndpointBaseUrl}rc/{command}?{queryStringKey}={queryStringValue}");
            return JsonConvert.DeserializeObject<RobotResultApi>(result.Content.ReadAsStringAsync().Result);
        }

        private RobotResultApi RoboExecutarRC(string command)
        {
            var result = _http.Get($"{Settings.RobotEndpointBaseUrl}rc/{command}");
            return JsonConvert.DeserializeObject<RobotResultApi>(result.Content.ReadAsStringAsync().Result);
        }
    }
}
