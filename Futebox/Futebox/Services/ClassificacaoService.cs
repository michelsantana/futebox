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

        public string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs)
        {

            var msg = $"{RoteiroDefaults.ObterSaudacao()} "
                + $"Veja a classificação do \"{CampeonatoUtils.ObterNomeDoCampeonato(processoClassificacaoArgs.campeonato)}\": "
                + $"Classificação atualizada {RoteiroDefaults.ObterHojeAmanhaOntem(processoClassificacaoArgs.dataExecucao.Value)}, {RoteiroDefaults.TraduzirDiaDoMes(processoClassificacaoArgs.dataExecucao.Value)}: "
                + $"Bora: ";

            var rnd = new Random();
            var indicePedirLike = rnd.Next(7, classificacao.Count() - 2);

            var range = processoClassificacaoArgs.range ?? new int[] { 0, 99 };

            if (processoClassificacaoArgs.temFases) classificacao = classificacao.Where(_ => _.fase == processoClassificacaoArgs.fase);

            if (processoClassificacaoArgs.classificacaoPorGrupos)
            {
                var grupos = classificacao.Select(_ => _.grupo).Distinct().ToList();

                grupos
                    .FindAll(_ => processoClassificacaoArgs.grupos.Contains(_))
                    .ToList()
                    .ForEach(g =>
                        {
                            msg += $"No Grupo {g}: ";
                            msg += "Ô ";
                            var classGrupo = classificacao.ToList().FindAll(_ => _.grupo == g);

                            classGrupo
                            .FindAll(_ => _.posicao >= (range[0]) && _.posicao <= (range[1]))
                            .OrderBy(_ => _.posicao)
                            .ToList()
                            .ForEach(_ =>
                            {
                                msg += $"{_.posicao}º colocado é, {_.time.ObterNomeWatson()}, "
                                + $"com {_.pontos} pontos, em { _.partidasJogadas} jogos: ";
                            });
                        });
                msg += "Muitíssimo obrigada a todos que assistiram até aqui: Até o próximo vídeo: ";
            }
            else
            {
                msg += "Ô ";
                classificacao
                    .Where(_ => _.posicao >= (range[0]) && _.posicao <= (range[1]))
                    .ToList()
                    .ForEach(_ =>
                        {

                            msg += $"{_.posicao}º colocado é, {_.time.ObterNomeWatson()}, "
                            + $"com {_.pontos} pontos, em { _.partidasJogadas} jogos: ";

                            if (~~_.posicao == indicePedirLike)
                            {
                                msg += "Meus parças: Já deixa aquela deedáda no laique e se inscrévi no canal: Continuando: Ô ";
                            }
                        });
                msg += "Muitíssimo obrigada a todos que assistiram até aqui: Até o próximo vídeo: ";
            }

            return msg;
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
                grupo = classificacao.grupo,
                fase = classificacao.fase
            };
            return retorno;
        }
    }
}
