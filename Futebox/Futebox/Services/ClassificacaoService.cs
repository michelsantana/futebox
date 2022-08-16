using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Futebox.Services
{
    public class ClassificacaoService : IClassificacaoService
    {
        readonly ICacheHandler _cache;
        readonly ITimeRepositorio _timeRepositorio;
        readonly IFootstatsService _footstatsService;

        public ClassificacaoService(ICacheHandler cache, ITimeRepositorio timeRepositorio, IFootstatsService footstatsService)
        {
            _cache = cache;
            _timeRepositorio = timeRepositorio;
            _footstatsService = footstatsService;
        }

        public IEnumerable<ClassificacaoVM> ObterClassificacaoPorCampeonato(EnumCampeonato campeonato, bool usarCache = true)
        {
            var cacheName = $"{nameof(FootstatsClassificacao)}{campeonato}";
            var resultado = _cache.ObterConteudo<List<FootstatsClassificacao>>(cacheName);
            if (resultado == null || resultado.Count == 0 || !usarCache)
            {
                resultado = _footstatsService.ObterClassificacaoServico(campeonato);
                _cache.DefinirConteudo(cacheName, resultado, 3);
            }

            //if (CampeonatoUtils.Config[campeonato].classificacaoPorGrupo && CampeonatoUtils.Config[campeonato].grupoFicticio)
            //{
            //    resultado.Take(resultado.Count / 2).ToList().ForEach(_ => _.grupo = "FAKE1");
            //    resultado.Skip(resultado.Count / 2).ToList().ForEach(_ => _.grupo = "FAKE2");
            //}

            return resultado
                .Select(_ => ConverterEmClassificacaoVM(_));
        }

        public IEnumerable<ClassificacaoVM> ObterClassificacaoPorCampeonatoFase(EnumCampeonato campeonato, string fase, bool usarCache = true)
        {
            var cacheName = $"{nameof(FootstatsClassificacao)}{campeonato}";
            var resultado = _cache.ObterConteudo<List<FootstatsClassificacao>>(cacheName);
            if (resultado == null || resultado.Count == 0 || !usarCache)
            {
                resultado = _footstatsService.ObterClassificacaoServico(campeonato);
                _cache.DefinirConteudo(cacheName, resultado, 3);
            }

            if (CampeonatoUtils.Config[campeonato].fases?.Count() > 1)
                resultado = resultado.GroupBy(_ => _.fase).First(_ => _.Key == fase).ToList();

            //if (CampeonatoUtils.Config[campeonato].classificacaoPorGrupo && CampeonatoUtils.Config[campeonato].grupoFicticio)
            //{
            //    resultado.Take(resultado.Count / 2).ToList().ForEach(_ => _.grupo = "FAKE1");
            //    resultado.Skip(resultado.Count / 2).ToList().ForEach(_ => _.grupo = "FAKE2");
            //}

            return resultado
                .Select(_ => ConverterEmClassificacaoVM(_));
        }

        public Tuple<string, string> ObterAtributosDoVideo(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs)
        {
            var camp = CampeonatoUtils.ObterNomeDoCampeonato(processoClassificacaoArgs.campeonato);
            var data = DateTime.Now.ToString("dd/MM/yyyy");
            var ano = DateTime.Now.ToString("yyyy");
            var titulo = $"CLASSIFICAÇÃO {camp} {ano} - {data} - ATUALIZADA";

            titulo = string.IsNullOrEmpty(processoClassificacaoArgs.titulo) ? titulo : processoClassificacaoArgs.titulo;

            var descricao = "";

            var palavraschave = new string[] {
                $"{camp}",
                $"TABELA {camp}",
                $"TABELA CLASSIFICAÇÃO",
                $"CLASSIFICAÇÃO {camp}",
                $"CAMPEONATO {camp}",
                $"FUTEBOL",
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
            });

            palavraschave.ToList().ForEach(_ =>
            {
                var semAcento = _?.RemoverAcentos2();
                descricao += $"#{semAcento.Replace(" ", "")}";
                descricao += "\n";
                descricao += $"#{semAcento.Replace(" ", "")}{ano}";
                descricao += "\n";
            });
            return Tuple.Create(titulo, descricao);
        }

        private ClassificacaoVM ConverterEmClassificacaoVM(FootstatsClassificacao classificacao)
        {
            var idEquipe = classificacao.idEquipe.ToString();
            var time = _timeRepositorio.GetSingle(_ => _.origem_ext_id == idEquipe);
            var retorno = new ClassificacaoVM()
            {
                brasao = time.logoBin,
                clube = time.nome,
                derrotas = classificacao.derrotas,
                empates = classificacao.empates,
                golsContra = classificacao.golsContra,
                golsPro = classificacao.golsPro,
                partidasJogadas = classificacao.jogos,
                pontos = classificacao.pontos,
                posicao = classificacao.posicao,
                saldoGols = classificacao.saldoDeGols,
                vitorias = classificacao.vitorias,
                time = time,
                grupo = classificacao.grupo,
                fase = classificacao.fase
            };
            return retorno;
        }
    }
}
