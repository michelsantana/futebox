using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static Futebox.Models.Enums;

namespace Futebox.Services
{
    public class FootstatsService : IFootstatsService
    {
        readonly IHttpHandler _http;
        readonly ILogger<IFootstatsService> _logger;

        public const string BaseURL = "http://apifutebol.footstats.com.br/";
        public const string token = "Bearer 72fa6abf-408";

        public FootstatsService(IHttpHandler http, ILogger<IFootstatsService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public List<FootstatsTime> ObterTimesServico()
        {
            var retorno = new List<FootstatsTime>();
            var campeonatos = new Campeonatos[] {
                Campeonatos.BrasileiraoSerieA,
                Campeonatos.BrasileiraoSerieB
            };
            foreach (var campeonato in campeonatos)
            {
                var rota = $"3.1/campeonatos/{(int)campeonato}/equipes";
                var respostaDaApi = _http.Get($"{BaseURL}/{rota}", MontarHeaderToken());

                if (respostaDaApi.IsSuccessStatusCode)
                {
                    var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                    var deserializado = DeserializarJson<FootstatsTime>(jsonRetornadoApi);
                    if (deserializado != null) retorno.AddRange(deserializado);
                }
            }
            return retorno;
        }

        public List<FootstatsClassificacao> ObterClassificacaoServico(Campeonatos campeonato)
        {
            var retorno = new List<FootstatsClassificacao>();

            var rota = $"3.1/campeonatos/{(int)campeonato}/classificacao";
            var respostaDaApi = _http.Get($"{BaseURL}/{rota}", MontarHeaderToken());

            if (respostaDaApi.IsSuccessStatusCode)
            {
                var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                var deserializado = DeserializarJson<FootstatsClassificacao>(jsonRetornadoApi);
                if (deserializado != null) retorno.AddRange(deserializado);
            }

            return retorno;
        }

        public List<FootstatsPartida> ObterPartidasHoje()
        {
            var retorno = new List<FootstatsPartida>();

            var rota = $"3.1/partidas/hoje";
            var respostaDaApi = _http.Get($"{BaseURL}/{rota}", MontarHeaderToken());

            if (respostaDaApi.IsSuccessStatusCode)
            {
                var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                var deserializado = DeserializarJson<FootstatsPartida>(jsonRetornadoApi);
                if (deserializado != null) retorno.AddRange(deserializado);
            }

            return retorno;
        }

        private Dictionary<string, string> MontarHeaderToken()
        {
            var header = new Dictionary<string, string>() { { "Authorization", token } };
            return header;
        }

        private IEnumerable<T> DeserializarJson<T>(string json)
        {
            try
            {
                var node = JObject.Parse(json).SelectToken("data").ToString();
                var listaFootstatsTime = JsonConvert.DeserializeObject<IEnumerable<T>>(node);
                return listaFootstatsTime;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao deserializar o conteúdo", ex);
                return null;
            }
        }
    }
}