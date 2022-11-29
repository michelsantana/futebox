using Futebox.Models;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Futebox.Services
{
    public class RodadaService : IRodadaService
    {
        IFootstatsService _footStatsService;
        ICacheHandler _cache;
        IPartidasService _partidasService;

        public RodadaService(ICacheHandler cache, IFootstatsService footStatsService, IPartidasService partidasService)
        {
            _cache = cache;
            _footStatsService = footStatsService;
            _partidasService = partidasService;
        }

        public List<PartidaVM> ObterPartidasDaRodada(Models.Enums.EnumCampeonato campeonato, int rodada, bool usarCache = true)
        {
            var cacheName = $"{nameof(PartidaVM)}-{campeonato}-{rodada}";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado.Count == 0 || !usarCache)
            {
                resultado = _footStatsService.ObterPartidasDaRodada(campeonato, rodada);
                _cache.DefinirConteudo(cacheName, resultado, 3);
            }
            return resultado.Select(_ => _partidasService.ConverterEmPartidaVM(_)).ToList();
        }        

        public Tuple<string, string> ObterAtributosDoVideo(ProcessoRodadaArgs processoRodadaArgs)
        {
            var camp = CampeonatoUtils.ObterNomeDoCampeonato(processoRodadaArgs.campeonato);
            var data = DateTime.Now.ToString("dd/MM/yyyy");
            var ano = DateTime.Now.ToString("yyyy");
            var titulo = $"PROGRAMAÇÃO - {processoRodadaArgs.rodada}ª RODADA - {camp} {ano} - {data} - ATUALIZADA";

            titulo = string.IsNullOrEmpty(processoRodadaArgs.titulo) ? titulo : processoRodadaArgs.titulo;

            var descricao = "";

            var palavraschave = new string[] {
                $"{camp}",
                $"RODADA {camp}",
                $"{processoRodadaArgs.rodada}ª RODADA",
                $"CAMPEONATO {camp}"
            };

            palavraschave.ToList().ForEach(_ =>
            {
                var comAcento = _;
                var semAcento = comAcento?.RemoverAcentos2();

                descricao += comAcento;
                descricao += "\n";
                descricao += $"{comAcento} {ano}";
                descricao += "\n";
                descricao += semAcento;
                descricao += "\n";
                descricao += $"{semAcento} {ano}";
                descricao += "\n";
                descricao += $"#[esc]{comAcento.Replace(" ", "")}";
                descricao += "\n";
                descricao += $"#[esc]{comAcento.Replace(" ", "")}{ano}";
                descricao += "\n";
                descricao += $"#[esc]{semAcento.Replace(" ", "")}";
                descricao += "\n";
                descricao += $"#[esc]{semAcento.Replace(" ", "")}{ano}";
                descricao += "\n";
            });

            return Tuple.Create(titulo, descricao);
        }
    }
}
