using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Futebox.Models.Enums;
using Futebox.Pages.Shared;

namespace Futebox.Pages
{
    public class ClassificacaoModel : BaseViewModel
    {
        readonly IClassificacaoService _classificacaoService;

        public EnumCampeonato? campeonatoFoco = null;
        public EnumCampeonato[] campeonatosAtivos = CampeonatoUtils.ObterCampeonatosAtivos();
        public PageViewModes visualizacao = PageViewModes.padrao;

        public ClassificacaoModel(IClassificacaoService classificacaoService)
        {
            _classificacaoService = classificacaoService;
        }

        public void OnGet(PageViewModes visualizacao, string campeonato)
        {
            if (!string.IsNullOrEmpty(campeonato)) campeonatoFoco = ((Models.Enums.EnumCampeonato)int.Parse(campeonato));
            this.visualizacao = visualizacao;
        }

        public PartialViewResult OnGetCampeonato(int campeonato, string fase = null)
        {
            var partialModel = new PartialModel();
            var enumCampeonato = (EnumCampeonato)campeonato;

            partialModel.nomeCampeonato = CampeonatoUtils.ObterNomeDoCampeonato(enumCampeonato);

            if(string.IsNullOrEmpty(fase)) partialModel.classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(enumCampeonato, UsarCache(visualizacao))?.ToList();
            else partialModel.classificacao = _classificacaoService.ObterClassificacaoPorCampeonatoFase(enumCampeonato, fase, UsarCache(visualizacao))?.ToList();

            string partial;
            switch (enumCampeonato)
            {
                case EnumCampeonato.BrasileiraoSerieA:
                    IdentificarCoresDestaqueBrasileiraoSerieA(partialModel);
                    partial = "Templates/_tabelaDeClassificacao";
                    break;
                case EnumCampeonato.BrasileiraoSerieB:
                    IdentificarCoresDestaqueBrasileiraoSerieB(partialModel);
                    partial = "Templates/_tabelaDeClassificacao";
                    break;
                case EnumCampeonato.Libertadores2021:
                    partial = "Templates/_tabelaDeClassificacaoLibertadores";
                    break;
                case EnumCampeonato.Paulistao2022:
                    IdentificarCoresDestaquePaulistao(partialModel);
                    partial = "Templates/_tabelaDeClassificacaoLibertadores";
                    break;
                default:
                    partial = "Templates/_tabelaDeClassificacao";
                    break;
            }
            return Partial(partial, partialModel);
        }

        private void IdentificarCoresDestaqueBrasileiraoSerieA(PartialModel partial)
        {
            partial.legenda.Clear();
            foreach (var time in partial.classificacao)
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
            partial.legenda.Add("<span class='badge bg-info'>&nbsp;</span> Fase de grupos da Copa Libertadores");
            partial.legenda.Add("<span class='badge bg-warning'>&nbsp;</span> Qualificatórias da Copa Libertadores");
            partial.legenda.Add("<span class='badge bg-success'>&nbsp;</span> Fase de grupos da Copa Sul-Americana");
            partial.legenda.Add("<span class='badge bg-danger'>&nbsp;</span> Rebaixamento");
        }

        private void IdentificarCoresDestaqueBrasileiraoSerieB(PartialModel partial)
        {
            partial.legenda.Clear();
            foreach (var time in partial.classificacao)
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
            partial.legenda.Add("<span class='badge bg-info'>&nbsp;</span> Promoção");
            partial.legenda.Add("<span class='badge bg-danger'>&nbsp;</span> Rebaixamento");
        }

        private void IdentificarCoresDestaquePaulistao(PartialModel partial)
        {
            partial.legenda.Clear();
            foreach (var time in partial.classificacao)
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
            partial.legenda.Add("<span class='badge bg-info'>&nbsp;</span> Próxima rodada");
        }

        public class PartialModel
        {
            public string nomeCampeonato { get; set; }
            public string visualizacao { get; set; }
            public List<ClassificacaoVM> classificacao { get; set; }
            public List<string> legenda = new List<string>();
        }
    }
}
