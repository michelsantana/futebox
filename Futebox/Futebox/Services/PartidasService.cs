﻿using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models.Enums;
using Newtonsoft.Json;

namespace Futebox.Services
{
    public class PartidasService : IPartidasService
    {
        readonly ICacheHandler _cache;
        readonly IFootstatsService _footstatsService;
        readonly ITimeRepositorio _timeRepositorio;
        public PartidasService(ICacheHandler cache, IFootstatsService footstatsService, ITimeRepositorio timeRepositorio)
        {
            _cache = cache;
            _footstatsService = footstatsService;
            _timeRepositorio = timeRepositorio;
        }

        public PartidaVM ObterPartida(int idPartida, bool usarCache = true)
        {
            var hoje = DateTime.Now.ToString("yyyyMMdd");
            var cacheName = $"{nameof(FootstatsPartida)}-partida-{idPartida}";
            var resultado = _cache.ObterConteudo<FootstatsPartida>(cacheName);
            if (resultado == null || !usarCache)
            {
                resultado = _footstatsService.ObterPartida(idPartida);
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            return ConverterEmPartidaVM(resultado);
        }

        public IEnumerable<PartidaVM> ObterPartidasPeriodo(bool usarCache = true)
        {
            var hoje = DateTime.Now.ToString("yyyyMMdd");
            var cacheName = $"{nameof(FootstatsPartida)}-{hoje}-periodo";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado?.Count == 0 || !usarCache)
            {
                resultado = _footstatsService.ObterPartidasPeriodo();
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            var campeonatos = CampeonatoUtils.ObterCampeonatosAtivos().Select(_ => (int)_);
            resultado = resultado.FindAll(_ => campeonatos.Contains(_.idCampeonato));

            return resultado.Select(_ => ConverterEmPartidaVM(_));
        }

        public IEnumerable<PartidaVM> ObterPartidasHoje(bool usarCache = true)
        {
            var hoje = DateTime.Now.ToString("yyyyMMdd");
            var cacheName = $"{nameof(FootstatsPartida)}-{hoje}";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado?.Count == 0 || !usarCache)
            {
                resultado = _footstatsService.ObterPartidasHoje();
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            var campeonatos = CampeonatoUtils.ObterCampeonatosAtivos().Select(_ => (int)_);
            resultado = resultado.FindAll(_ => campeonatos.Contains(_.idCampeonato));

            return resultado.Select(_ => ConverterEmPartidaVM(_));
        }

        public IEnumerable<PartidaVM> ObterPartidasDaRodada(EnumCampeonato campeonato, int rodada, bool usarCache = true)
        {
            var hoje = DateTime.Now.ToString("yyyyMMdd");
            var cacheName = $"{nameof(FootstatsPartida)}-{hoje}";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado?.Count == 0 || !usarCache)
            {
                resultado = _footstatsService.ObterPartidasDaRodada(campeonato, rodada);
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            return resultado.Select(_ => ConverterEmPartidaVM(_));
        }

        public IEnumerable<PartidaVM> ObterPartidasDoCampeonato(Models.Enums.EnumCampeonato campeonato, bool usarCache = true)
        {
            var hoje = DateTime.Now.ToString("yyyyMM");
            var cacheName = $"{nameof(FootstatsPartida)}-{hoje}-{campeonato}";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado?.Count == 0 || !usarCache)
            {
                resultado = _footstatsService.ObterPartidasDoCampeonato(campeonato);
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            return resultado.Select(_ => ConverterEmPartidaVM(_));
        }

        public IEnumerable<PartidaVM> ObterPartidasAntigas(string cacheFile)
        {
            return _cache.ObterConteudoExpirado<List<FootstatsPartida>>(cacheFile).Select(_ => ConverterEmPartidaVM(_));
        }

        public string ObterRoteiroDaPartida(int idPartida)
        {
            var partida = ObterPartida(idPartida);
            return ObterRoteiroDaPartida(partida);
        }

        public string ObterRoteiroDaPartida(PartidaVM partida)
        {
            var empate = int.Parse(partida.golsMandante) == int.Parse(partida.golsVisitante);

            var vencedor = int.Parse(partida.golsMandante) > int.Parse(partida.golsVisitante) ? partida.timeMandante : partida.timeVisitante;
            var perdedor = int.Parse(partida.golsMandante) < int.Parse(partida.golsVisitante) ? partida.timeMandante : partida.timeVisitante;

            string mensagem =
                empate ? RoteiroDefaults.ObterTrechoEmpate(partida)
                    : RoteiroDefaults.ObterTrechoVencedor(partida, vencedor, perdedor);

            return //$"{RoteiroDefaults.ObterSaudacao()}\n" +
                $"{mensagem}";
        }

        public Tuple<string, string> ObterAtributosDoVideo(PartidaVM partida, ProcessoPartidaArgs processoPartidaArgs)
        {
            var titulo = $"{partida.timeMandante.nome} x {partida.timeVisitante.nome} " +
                $"- {partida.dataPartida.ToString("dd/MM/yyyy")}" +
                $"- {partida.campeonato}! " +
                $"Quem venceu!? #shorts";
            titulo = string.IsNullOrEmpty(processoPartidaArgs.titulo) ? titulo : processoPartidaArgs.titulo;

            var descricao = $"Fala meus parças resultado da partida. Espero que gostem!";

            return Tuple.Create(titulo, descricao);
        }

        public PartidaVM ConverterEmPartidaVM(FootstatsPartida footstatsPartida)
        {
            try
            {

                var idExternoMandante = footstatsPartida.idEquipeMandante.ToString();
                var idExternoVisitante = footstatsPartida.idEquipeVisitante.ToString();

                var timeMandante = _timeRepositorio.GetSingle(_ => _.origem_ext_id == idExternoMandante);
                var timeVisitante = _timeRepositorio.GetSingle(_ => _.origem_ext_id == idExternoVisitante);
                var dtPartida =
                    new DateTime(footstatsPartida.dataDaPartida?.year ?? 1990, 1, 1)
                    .AddDays((footstatsPartida.dataDaPartida?.dayOfYear ?? 1) - 1)
                    .AddHours(footstatsPartida.dataDaPartida?.hour ?? 0)
                    .AddMinutes(footstatsPartida.dataDaPartida?.minute ?? 0);
                return new PartidaVM()
                {
                    idExterno = footstatsPartida.id,
                    timeMandante = timeMandante,
                    timeVisitante = timeVisitante,
                    golsMandante = string.IsNullOrEmpty(footstatsPartida.placar.golsMandante) ? "0" : footstatsPartida.placar.golsMandante,
                    golsVisitante = string.IsNullOrEmpty(footstatsPartida.placar.golsVisitante) ? "0" : footstatsPartida.placar.golsVisitante,
                    dataHoraDaPartida = dtPartida.ToString("dddd, dd MMMM yyyy - HH:mm"),
                    dataPartida = dtPartida,
                    estadio = footstatsPartida.estadio,
                    campeonato = CampeonatoUtils.ObterNomeDoCampeonato((Models.Enums.EnumCampeonato)footstatsPartida.idCampeonato),
                    rodada = footstatsPartida.rodada
                };
            }
            catch (Exception ex)
            {
                EyeLog.Log($"Ainda não identificado {footstatsPartida.idEquipeMandante}x{footstatsPartida.idEquipeVisitante}");
                EyeLog.Log(JsonConvert.SerializeObject(footstatsPartida));
                EyeLog.Log(ex);
                return new PartidaVM()
                {
                    idExterno = footstatsPartida.id,
                    timeMandante = Time.Indefinido(),
                    timeVisitante = Time.Indefinido(),
                    golsMandante = "404",
                    golsVisitante = "404",
                    dataHoraDaPartida = "404",
                    dataPartida = DateTime.Now.AddYears(-20),
                    estadio = footstatsPartida.estadio,
                    campeonato = CampeonatoUtils.ObterNomeDoCampeonato((Models.Enums.EnumCampeonato)footstatsPartida.idCampeonato),
                    rodada = footstatsPartida.rodada
                };
            }
        }
    }
}
