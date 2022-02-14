using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Futebox.Models.Enums;

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

            resultado = resultado.GroupBy(_ => _.fase).First(_ => _.Key == fase).ToList();

            return resultado
                .Select(_ => ConverterEmClassificacaoVM(_));
        }

        public string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, EnumCampeonato campeonato)
        {

            var msg = $"{RoteiroDefaults.ObterSaudacao()} "
                + $"Veja agora a classificação do \"{CampeonatoUtils.ObterNomeDoCampeonato(campeonato)}\": "
                + $"Lembrando que essa, é a classificação no dia de hoje, {RoteiroDefaults.TraduzirDiaDoMes(DateTime.Now)}: "
                + $"Bora: "
                + "Ô ";

            var rnd = new Random();
            var indicePedirLike = rnd.Next(7, classificacao.Count() - 2);

            classificacao.ToList().ForEach(_ =>
            {

                msg += $"{_.posicao}º colocado é, {_.time.ObterNomeWatson()}, "
                + $"com {_.pontos} pontos, em { _.partidasJogadas} jogos: ";

                if (~~_.posicao == indicePedirLike)
                {
                    msg += "Meus parças: Já deixa aquela deedáda no laique e se inscrévi no canal: Continuando: Ô ";
                }
            });
            msg += "Muitíssimo obrigada a todos que assistiram até aqui: Até o próximo vídeo: ";
            return msg;
        }

        public Tuple<string, string> ObterAtributosDoVideo(IEnumerable<ClassificacaoVM> classificacao, EnumCampeonato campeonato)
        {
            var camp = CampeonatoUtils.ObterNomeDoCampeonato(campeonato);
            var data = DateTime.Now.ToString("dd/MM/yyyy");
            var ano = DateTime.Now.ToString("yyyy");
            var titulo = $"CLASSIFICAÇÃO {camp} {ano} - {data} - ATUALIZADA";

            var descricao = "";

            var palavraschave = new string[] {
                $"{camp}",
                $"TABELA {camp}",
                $"TABELA CLASSIFICAÇÃO",
                $"CLASSIFICAÇÃO {camp}",
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
                grupo = classificacao.grupo
            };
            return retorno;
        }
    }
}
