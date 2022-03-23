using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Futebox.Pages
{
    public class MiniaturasModel : PageModel
    {

        IRodadaService _rodadaService;
        IClassificacaoService _classificacaoService;
        IPartidasService _partidaService;

        public CategoriaVideo tipo;

        public int width;
        public int height;

        public RodadaHandlerModel rodada;
        public ClassificacaoHandlerModel classificacao;
        public PartidaHandlerModel partida;

        public MiniaturasModel(IRodadaService rodadaService, IClassificacaoService classificacaoService, IPartidasService partidaService)
        {
            _rodadaService = rodadaService;
            _classificacaoService = classificacaoService;
            _partidaService = partidaService;
        }

        public void OnGet(CategoriaVideo t, string q, int w, int h)
        {
            tipo = t;
            switch (tipo)
            {
                case CategoriaVideo.partida:
                    PartidaHandler(q, w, h);
                    break;
                case CategoriaVideo.classificacao:
                    ClassificacaoHandler(q, w, h);
                    break;
                case CategoriaVideo.rodada:
                    RodadaHandler(q, w, h);
                    break;
                default:
                    break;
            }
        }

        public void RodadaHandler(string q, int w, int h)
        {
            var args = JsonConvert.DeserializeObject<ProcessoRodadaArgs>(q);
            width = w;
            height = h;
            var partidas = _rodadaService
                .ObterPartidasDaRodada(args.campeonato, args.rodada, true)
                .FindAll(_ => args.partidas.Contains(_.idExterno))
                .OrderBy(_ => _.dataPartida).ToList();

            rodada = new RodadaHandlerModel(args, width, height, partidas);
        }

        public void PartidaHandler(string q, int w, int h)
        {
            var args = JsonConvert.DeserializeObject<ProcessoPartidaArgs>(q);
            width = w;
            height = h;
            var partidaVm = _partidaService.ObterPartida(args.partida, false);

            partida = new PartidaHandlerModel(args, width, height, partidaVm);
        }

        public void ClassificacaoHandler(string q, int w, int h)
        {
            var args = JsonConvert.DeserializeObject<ProcessoClassificacaoArgs>(q);
            width = w;
            height = h;

            List<ClassificacaoVM> data = null;
            if (args.temFases)
                data = _classificacaoService.ObterClassificacaoPorCampeonatoFase(args.campeonato, args.fase, false).ToList();
            else
                data = _classificacaoService.ObterClassificacaoPorCampeonato(args.campeonato, false).ToList();

            classificacao = new ClassificacaoHandlerModel(args, width, height, args.grupos, data);
        }

        public class RodadaHandlerModel
        {
            public ProcessoRodadaArgs args;
            public int width;
            public int height;
            public List<PartidaVM> partidas;

            private int partidaAtualId = 0;
            private PartidaVM partidaAtual = null;

            public RodadaHandlerModel(ProcessoRodadaArgs args, int width, int height, List<PartidaVM> partidas)
            {
                this.args = args;
                this.width = width;
                this.height = height;
                this.partidas = partidas;
            }

            public PartidaVM Current()
            {
                return partidaAtual;
            }

            public void Next(bool infiniteNavigation = false)
            {
                if (partidaAtualId == 0) partidaAtual = partidas.First();
                else partidaAtual = partidas.SkipWhile(x => x.idExterno != partidaAtualId)
                   .Skip(1)
                   .DefaultIfEmpty(infiniteNavigation ? partidas[0] : null)
                   .FirstOrDefault();

                if (partidaAtual != null) partidaAtualId = partidaAtual.idExterno;
            }

            public void Prev(bool infiniteNavigation = false)
            {
                if (partidaAtualId == 0) partidaAtual = partidas.Last();
                else partidaAtual = partidas.TakeWhile(x => x.idExterno != partidaAtualId)
                    .DefaultIfEmpty(infiniteNavigation ? partidas[partidas.Count - 1] : null)
                    .LastOrDefault();

                if (partidaAtual != null) partidaAtualId = partidaAtual.idExterno;
            }
        }

        public class PartidaHandlerModel
        {
            public ProcessoPartidaArgs args;
            public int width;
            public int height;
            public PartidaVM partida;

            public PartidaHandlerModel(ProcessoPartidaArgs args, int width, int height, PartidaVM partida)
            {
                this.args = args;
                this.width = width;
                this.height = height;
                this.partida = partida;
            }
        }

        public class ClassificacaoHandlerModel
        {
            public ProcessoClassificacaoArgs args;
            public int width;
            public int height;
            public string[] grupos;
            public IEnumerable<IGrouping<string, ClassificacaoVM>> classificacao;
            public string legenda { get; set; }

            string currentGroup = null;

            public ClassificacaoHandlerModel(ProcessoClassificacaoArgs args, int width, int height, string[] grupos, List<ClassificacaoVM> classificacao)
            {
                this.args = args;
                this.width = width;
                this.height = height;
                this.grupos = grupos;

                if (args.classificacaoPorGrupos) classificacao = classificacao.FindAll(_ => args.grupos.Contains(_.grupo));

                this.classificacao = classificacao.GroupBy(_ => _.grupo);

                IdentificarLegenda();
            }

            public void IdentificarLegenda()
            {
                switch (args.campeonato)
                {
                    case EnumCampeonato.Indefinido:
                        legenda = null;
                        break;
                    case EnumCampeonato.BrasileiraoSerieA:
                        legenda = IdentificarCoresDestaqueBrasileiraoSerieA();
                        break;
                    case EnumCampeonato.BrasileiraoSerieB:
                        legenda = IdentificarCoresDestaqueBrasileiraoSerieB();
                        break;
                    case EnumCampeonato.Libertadores2021:
                        legenda = null;
                        break;
                    case EnumCampeonato.Paulistao2022:
                        legenda = IdentificarCoresDestaquePaulistao();
                        break;
                    default:
                        legenda = null;
                        break;
                }
            }

            private string IdentificarCoresDestaqueBrasileiraoSerieA()
            {
                var sbBuilder = new StringBuilder();
                foreach (var group in classificacao)
                {
                    foreach (var time in group)
                    {
                        if (time.posicao < 5)
                        {
                            time.corDestaque = "azul";
                            continue;
                        }
                        if (time.posicao < 7)
                        {
                            time.corDestaque = "laranja";
                            continue;
                        }
                        if (time.posicao < 13)
                        {
                            time.corDestaque = "verde";
                            continue;
                        }
                        if (time.posicao < 18)
                        {
                            time.corDestaque = "branco";
                            continue;
                        }
                        if (time.posicao < 21)
                        {
                            time.corDestaque = "vermelho";
                            continue;
                        }
                    }
                }
                sbBuilder.AppendLine("<span><span class='badge bg-info'>&nbsp;</span> Fase de grupos da Copa Libertadores</span>");
                sbBuilder.AppendLine("<span><span class='badge bg-warning'>&nbsp;</span> Qualificatórias da Copa Libertadores</span>");
                sbBuilder.AppendLine("<span><span class='badge bg-success'>&nbsp;</span> Fase de grupos da Copa Sul-Americana</span>");
                sbBuilder.AppendLine("<span><span class='badge bg-danger'>&nbsp;</span> Rebaixamento</span>");
                return sbBuilder.ToString();
            }

            private string IdentificarCoresDestaqueBrasileiraoSerieB()
            {
                var sbBuilder = new StringBuilder();
                foreach (var group in classificacao)
                {
                    foreach (var time in group)
                    {
                        if (time.posicao < 5)
                        {
                            time.corDestaque = "azul";
                            continue;
                        }
                        if (time.posicao >= 5 && time.posicao <= 16)
                        {
                            time.corDestaque = "branco";
                            continue;
                        }
                        if (time.posicao > 16 && time.posicao < 21)
                        {
                            time.corDestaque = "vermelho";
                            continue;
                        }
                    }
                }
                sbBuilder.AppendLine("<span><span class='badge bg-info'>&nbsp;</span> Promoção</span>");
                sbBuilder.AppendLine("<span><span class='badge bg-danger'>&nbsp;</span> Rebaixamento</span>");
                sbBuilder.AppendLine("<span>&nbsp;</span>");
                return sbBuilder.ToString();
            }

            private string IdentificarCoresDestaquePaulistao()
            {
                var sbBuilder = new StringBuilder();
                foreach (var group in classificacao)
                {
                    foreach (var time in group)
                    {
                        if (time.posicao == 1 || time.posicao == 2)
                        {
                            time.corDestaque = "azul";
                            continue;
                        }
                        else
                        {
                            time.corDestaque = "branco";
                            continue;
                        }
                    }
                }
                sbBuilder.AppendLine("<span><span class='badge bg-info'>&nbsp;</span> Próxima rodada</span>");
                sbBuilder.AppendLine("<span>&nbsp;</span>");
                sbBuilder.AppendLine("<span>&nbsp;</span>");
                return sbBuilder.ToString();
            }


            public string CurrentGroup()
            {
                return currentGroup;
            }

            public void NextGroup(bool infiniteNavigation = false)
            {
                if (currentGroup == null) currentGroup = grupos.First();
                else currentGroup = grupos.SkipWhile(x => x != currentGroup)
                   .Skip(1)
                   .DefaultIfEmpty(infiniteNavigation ? grupos[0] : null)
                   .FirstOrDefault();
            }
        }
    }
}
