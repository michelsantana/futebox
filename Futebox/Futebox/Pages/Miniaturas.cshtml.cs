using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Futebox.Pages
{
    public class MiniaturasModel : PageModel
    {

        IRodadaService _rodadaService;

        public CategoriaVideo tipo;
        public ProcessoRodadaArgs args;
        public int width;
        public int height;

        public RodadaHandlerModel rodada;

        public MiniaturasModel(IRodadaService rodadaService)
        {
            _rodadaService = rodadaService;
        }

        public void OnGet(CategoriaVideo t, string q, int w, int h)
        {
            tipo = t;
            switch (tipo)
            {
                case CategoriaVideo.partida:
                    break;
                case CategoriaVideo.classificacao:
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
            args = JsonConvert.DeserializeObject<ProcessoRodadaArgs>(q);
            width = w;
            height = h;
            var partidas = _rodadaService
                .ObterPartidasDaRodada(args.campeonato, args.rodada, true)
                .FindAll(_ => args.partidas.Contains(_.idExterno));

            rodada = new RodadaHandlerModel(args, width, height, partidas);
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

    }
}
