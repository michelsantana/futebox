﻿using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Futebox.Models.Enums;

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
            var campeonatos = CampeonatoUtils.ObterCampeonatosAtivos();
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

        public List<FootstatsClassificacao> ObterClassificacaoServico(Models.Enums.EnumCampeonato campeonato)
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

        public FootstatsPartida ObterPartida(int partida)
        {
            var retorno = new FootstatsPartida();

            var rota = $"3.1/partidas/{partida}";
            var respostaDaApi = _http.Get($"{BaseURL}/{rota}", MontarHeaderToken());

            if (respostaDaApi.IsSuccessStatusCode)
            {
                var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                var deserializado = DeserializarSingleJson<FootstatsPartida>(jsonRetornadoApi);
                if (deserializado != null) retorno = deserializado;
            }

            return retorno;
        }

        public List<FootstatsPartida> ObterPartidasPeriodo()
        {
            var retorno = new List<FootstatsPartida>();

            var rota = $"3.1/partidas/periodo";
            var respostaDaApi = _http.Get($"{BaseURL}/{rota}", MontarHeaderToken());

            if (respostaDaApi.IsSuccessStatusCode)
            {
                var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                var deserializado = DeserializarJson<FootstatsPartida>(jsonRetornadoApi);
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

        public List<FootstatsPartida> ObterPartidasDaRodada(Models.Enums.EnumCampeonato campeonato, int rodada)
        {
            var retorno = new List<FootstatsPartida>();

            var rota = $"3.1/campeonatos/{(int)campeonato}/partidas/rodada/{rodada}";
            var respostaDaApi = _http.Get($"{BaseURL}/{rota}", MontarHeaderToken());

            if (respostaDaApi.IsSuccessStatusCode)
            {
                var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                var deserializado = DeserializarJson<FootstatsPartida>(jsonRetornadoApi);
                if (deserializado != null) retorno.AddRange(deserializado);
            }
            return retorno;
        }

        public List<FootstatsPartida> ObterPartidasDoCampeonato(Models.Enums.EnumCampeonato campeonato)
        {
            var retorno = new List<FootstatsPartida>();

            var rota = $"3.1/campeonatos/{(int)campeonato}/partidas";
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

        private T DeserializarSingleJson<T>(string json)
        {
            try
            {
                var node = JObject.Parse(json).SelectToken("data").ToString();
                var listaFootstatsTime = JsonConvert.DeserializeObject<T>(node);
                return listaFootstatsTime;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao deserializar o conteúdo", ex);
                return default(T);
            }
        }
    }
}