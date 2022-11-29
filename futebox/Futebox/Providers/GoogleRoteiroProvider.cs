using Futebox.Models;
using Futebox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Providers
{
    public class GoogleRoteiroProvider : IRoteiroProvider
    {
        private string ObterSaudacao() => $"Fala torcedor e torcedora, bem vindo ao canal fute box. ";
        private string ObterSaudacao2() => $"Fala meus chegados: ";

        private static string ObterTrechoEmpate(PartidaVM partida)
        {
            return //$"Hoje: " +
                    $"{partida.timeMandante.ObterNomeWatson()} e {partida.timeVisitante.ObterNomeWatson()}, " +
                    $"empataram por {partida.golsMandante} a {partida.golsMandante} no estádio, {partida.estadio}. " +
                    $"Esse foi um jogo do campeonato, {partida.campeonato}: " +
                    $"e iniciou por volta das {RoteiroDefaults.TraduzirHoras(partida.dataPartida)}. " +
                    $"Pra quem você estava torcendo? deixa aqui nos comentários junto com aquela dedada no laique.";
        }

        private static string ObterTrechoVencedor(PartidaVM partida, Time vencedor, Time perdedor)
        {
            var golsVencedor = partida.timeMandante.sigla == vencedor.sigla ? partida.golsMandante : partida.golsVisitante;
            var golsPerdedor = partida.timeMandante.sigla == perdedor.sigla ? partida.golsMandante : partida.golsVisitante;

            return $"{vencedor.ObterNomeWatson()} venceu {perdedor.ObterNomeWatson()}: " +
                    $"por {golsVencedor} a {golsPerdedor} no estádio {partida.estadio}. " +
                    $"Esse foi um jogo do campeonato, {partida.campeonato}: " +
                    $"e iniciou {RoteiroDefaults.ObterHojeAmanhaOntem(partida.dataPartida)} por volta das {RoteiroDefaults.TraduzirHoras(partida.dataPartida)}. " +
                    $"Pra quem você estava torcendo? deixa aqui nos comentários junto com aquela dedada no laique.";
        }

        private bool IdentificaClassico(Time timeMandante, Time timeVisitante)
        {
            return false;
            return ListaDeClassicos.ObterLista()
                .FindAll(_ =>
                _.atalho.Any(a => a == timeMandante.sigla) && _.atalho.Any(a => a == timeVisitante.sigla))
                .Count > 0;
        }

        public string ObterRoteiroDaClassificacao(IEnumerable<ClassificacaoVM> classificacao, ProcessoClassificacaoArgs processoClassificacaoArgs)
        {

            var msg = "";

            if (processoClassificacaoArgs.social == Models.Enums.RedeSocialFinalidade.YoutubeShorts)
            {
                msg += $""
               + $"Veja a classificação do \"{CampeonatoUtils.ObterNomeDoCampeonato(processoClassificacaoArgs.campeonato)}\": "
               + $"Atualizada {RoteiroDefaults.ObterHojeAmanhaOntem(processoClassificacaoArgs.dataExecucao.Value)}, {RoteiroDefaults.TraduzirDiaDoMes(processoClassificacaoArgs.dataExecucao.Value)}: "
               + $"Bora: ";
            }
            else
            {
                msg += $"{ObterSaudacao()} ";
                msg += $""
                + $"Veja a classificação do \"{CampeonatoUtils.ObterNomeDoCampeonato(processoClassificacaoArgs.campeonato)}\": "
                + $"Atualizada {RoteiroDefaults.ObterHojeAmanhaOntem(processoClassificacaoArgs.dataExecucao.Value)}, {RoteiroDefaults.TraduzirDiaDoMes(processoClassificacaoArgs.dataExecucao.Value)}: "
                + $"Bora: ";
            }

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
                        msg += "";
                        var classGrupo = classificacao.ToList()
                            .FindAll(_ => _.grupo == g);

                        classGrupo
                    .FindAll(_ => _.posicao >= (range[0]) && _.posicao <= (range[1]))
                    .OrderBy(_ => _.posicao)
                    .ToList()
                    .ForEach(_ =>
                            {
                                msg += $"{_.posicao}º colocado, {_.time.ObterNomeWatson()}, "
                                + $"com {_.pontos} pontos, em { _.partidasJogadas} jogos: ";
                            });
                    });
                msg += "Muitíssimo obrigada a todos que assistiram até aqui: Até o próximo vídeo: ";
            }
            else
            {
                msg += "";
                classificacao
                    .Where(_ => _.posicao >= (range[0]) && _.posicao <= (range[1]))
                    .ToList()
                    .ForEach(_ =>
                    {

                        msg += $"{_.posicao}º colocado, {_.time.ObterNomeWatson()}, "
                    + $"com {_.pontos} pontos, em { _.partidasJogadas} jogos: ";

                        if (~~_.posicao == indicePedirLike)
                        {
                            msg += "Meus parças: Já deixa aquela dedada no laique e se inscrévi no canal: Continuando: Ô ";
                        }
                    });
                msg += "Muitíssimo obrigada a todos que assistiram até aqui: Até o próximo vídeo: ";
            }

            return msg;
        }

        public string ObterRoteiroDaPartida(PartidaVM partida)
        {
            var empate = int.Parse(partida.golsMandante) == int.Parse(partida.golsVisitante);

            var vencedor = int.Parse(partida.golsMandante) > int.Parse(partida.golsVisitante) ? partida.timeMandante : partida.timeVisitante;
            var perdedor = int.Parse(partida.golsMandante) < int.Parse(partida.golsVisitante) ? partida.timeMandante : partida.timeVisitante;

            string mensagem =
                empate ? ObterTrechoEmpate(partida)
                    : ObterTrechoVencedor(partida, vencedor, perdedor);

            return $"{mensagem}";
        }

        public string ObterRoteiroDaRodada(IEnumerable<PartidaVM> partidas, ProcessoRodadaArgs processoRodadaArgs)
        {
            var roteiro = "";
            if (processoRodadaArgs.social == Models.Enums.RedeSocialFinalidade.YoutubeShorts)
                roteiro += ObterSaudacao();

            roteiro += $"Veja a programação dos jogos da {processoRodadaArgs.rodada}ª rodada do {CampeonatoUtils.ObterNomeDoCampeonato(processoRodadaArgs.campeonato)}: ";

            partidas
                .Where(_ => processoRodadaArgs.partidas.Contains(_.idExterno))
                .OrderBy(_ => _.dataPartida)
                .GroupBy(_ => _.dataPartida.ToString("yyyyMMdd"))
                .ToList().ForEach(gp =>
                {
                    var dia = gp.FirstOrDefault().dataPartida;
                    roteiro += $"{dia.ToString("dddd")}, {dia.ToString("dd")} de {dia.ToString("MMMM")}: ";
                    gp.ToList().ForEach(_ =>
                    {
                        roteiro += $"{_.timeMandante.ObterNomeWatson()} e {_.timeVisitante.ObterNomeWatson()}: ";

                        if (IdentificaClassico(_.timeMandante, _.timeVisitante)) roteiro += $"Um clássico do futebol brasileiro: ";

                        roteiro += $"às {RoteiroDefaults.TraduzirHoras(_.dataPartida)}: ";
                        roteiro += $"no estádio {_.estadio}: ";
                    });
                });
            roteiro += $"Pra qual time você torce?: deixa aqui nos comentários junto com aquela dedada no laique. Até a próxima. ";
            return roteiro;
        }

        public string ObterRoteiroDoJogosDoDia(IEnumerable<PartidaVM> partidas, ProcessoJogosDiaArgs args)
        {
            var roteiro = ObterSaudacao();
            var referencia = DateTime.Now.AddDays(args.dataRelativa ?? 0);
            roteiro += $"Vejam agora os jogos de {RoteiroDefaults.ObterHojeAmanhaOntem(referencia)}: ";
            partidas = partidas.Where(_ => args.partidas.Contains(_.idExterno));

            var grupos = partidas.Select(_ => _.campeonato).Distinct().ToList();

            if (grupos.Count > 1)
            {

                grupos.ForEach(g =>
                {
                    roteiro += $"Do {g}:";
                    partidas
                        .ToList()
                        .FindAll(_ => _.campeonato == g)
                        .ForEach(_ =>
                        {
                            roteiro += $"{_.timeMandante.ObterNomeWatson()} e {_.timeVisitante.ObterNomeWatson()}: ";

                            if (IdentificaClassico(_.timeMandante, _.timeVisitante)) roteiro += $"Um clássico do futebol brasileiro: ";

                            roteiro += $"às {RoteiroDefaults.TraduzirHoras(_.dataPartida)}: ";
                            roteiro += $"no estádio {_.estadio}: ";
                        });
                });

                roteiro += $"Pra quem você torce?: deixa aqui nos comentários junto com aquela dedada no laique. Até a próxima. ";
            }
            else
            {
                partidas
               .Where(_ => args.partidas.Contains(_.idExterno))
               .OrderBy(_ => _.dataPartida)
               .GroupBy(_ => _.dataPartida.ToString("yyyyMMdd"))
               .ToList().ForEach(gp =>
               {
                   var dia = gp.FirstOrDefault().dataPartida;
                   gp.ToList().ForEach(_ =>
                   {
                       roteiro += $"{_.timeMandante.ObterNomeWatson()} e {_.timeVisitante.ObterNomeWatson()}: ";

                       if (IdentificaClassico(_.timeMandante, _.timeVisitante)) roteiro += $"Um clássico do futebol brasileiro: ";

                       roteiro += $"às {RoteiroDefaults.TraduzirHoras(_.dataPartida)}: ";
                       roteiro += $"no estádio {_.estadio}: ";
                   });
               });
                roteiro += $"Pra quem você torce?: deixa aqui nos comentários junto com aquela dedada no laique. Até a próxima. ";
            }

            return roteiro;
        }
    }
}
