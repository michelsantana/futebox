using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public string ObterRoteiroDaRodada(IEnumerable<PartidaVM> partidas, ProcessoRodadaArgs processoRodadaArgs)
        {
            var roteiro = RoteiroDefaults.ObterSaudacao();
            roteiro += $"Veja agora a programação dos jogos da {processoRodadaArgs.rodada}ª rodada do {CampeonatoUtils.ObterNomeDoCampeonato(processoRodadaArgs.campeonato)}: ";

            partidas
                .Where(_ => processoRodadaArgs.partidas.Contains(_.idExterno))
                .OrderBy(_ => _.dataPartida)
                .GroupBy(_ => _.dataPartida.ToString("yyyyMMdd"))
                .ToList().ForEach(gp =>
                {
                    var dia = gp.FirstOrDefault().dataPartida;
                    roteiro += $"{dia.ToString("D")}: ";
                    gp.ToList().ForEach(_ =>
                    {
                        roteiro += $"{_.timeMandante.ObterNomeWatson()} e {_.timeVisitante.ObterNomeWatson()}: ";

                        if (IdentificaClassico(_.timeMandante, _.timeVisitante)) roteiro += $"Um clássico do futebol brasileiro: ";

                        roteiro += $"às {RoteiroDefaults.TraduzirHoras(_.dataPartida)}: ";
                        roteiro += $"no estádio {_.estadio}: ";
                    });
                });
            roteiro += $"Pra qual time você torce?: deixa aqui nos comentários junto com aquela deedáda no laique. Até a próxima. ";
            return roteiro;
        }

        private bool IdentificaClassico(Time timeMandante, Time timeVisitante)
        {
            return ListaDeClassicos.ObterLista()
                .FindAll(_ =>
                _.atalho.Any(a => a == timeMandante.sigla) && _.atalho.Any(a => a == timeVisitante.sigla))
                .Count > 0;
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
                var semAcento = RoteiroDefaults.RemoverAcentos(comAcento);

                descricao += comAcento;
                descricao += "\n";
                descricao += $"{comAcento} {ano}";
                descricao += "\n";
                descricao += semAcento;
                descricao += "\n";
                descricao += $"{semAcento} {ano}";
                descricao += "\n";
                descricao += $"#{comAcento.Replace(" ", "")}";
                descricao += "\n";
                descricao += $"#{comAcento.Replace(" ", "")}{ano}";
                descricao += "\n";
                descricao += $"#{semAcento.Replace(" ", "")}";
                descricao += "\n";
                descricao += $"#{semAcento.Replace(" ", "")}{ano}";
                descricao += "\n";
            });

            return Tuple.Create(titulo, descricao);
        }
    }
}
