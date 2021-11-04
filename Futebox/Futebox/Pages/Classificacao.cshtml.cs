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

        public List<ClassificacaoVM> classificacao;
        public string nomeCampeonato;
        public Campeonatos? campeonatoFoco = null;
        public List<string> legenda = new List<string>();
        private bool clearCache = false;

        public ClassificacaoModel(IClassificacaoService classificacaoService)
        {
            _classificacaoService = classificacaoService;
        }

        public void OnGet(string campeonato, PageViewModes viewMode)
        {
            clearCache = viewMode == PageViewModes.print;
            classificacao = new List<ClassificacaoVM>();
            if (!string.IsNullOrEmpty(campeonato))
                campeonatoFoco = ((Campeonatos)int.Parse(campeonato));
        }

        public PartialViewResult OnGetCampeonato(int campeonato)
        {
            var enumCampeonato = (Campeonatos)campeonato;
            nomeCampeonato = CampeonatoUtils.ObterNomeDoCampeonato(enumCampeonato);
            classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(enumCampeonato, clearCache)?.ToList();
            legenda.Clear();
            var partial = "Templates/_tabelaDeClassificacao";
            switch (enumCampeonato)
            {
                case Campeonatos.BrasileiraoSerieA:
                    IdentificarCoresDestaqueBrasileiraoSerieA();
                    break;
                case Campeonatos.BrasileiraoSerieB:
                    IdentificarCoresDestaqueBrasileiraoSerieB();
                    break;
                case Campeonatos.Libertadores2021:
                    partial = "Templates/_tabelaDeClassificacaoLibertadores";
                    break;
            }
            return Partial(partial, this);
        }

        private void IdentificarCoresDestaqueBrasileiraoSerieA()
        {
            foreach (var time in classificacao)
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
            legenda.Add("<span class='badge bg-info'>&nbsp;</span> Fase de grupos da Copa Libertadores");
            legenda.Add("<span class='badge bg-warning'>&nbsp;</span> Qualificatórias da Copa Libertadores");
            legenda.Add("<span class='badge bg-success'>&nbsp;</span> Fase de grupos da Copa Sul-Americana");
            legenda.Add("<span class='badge bg-danger'>&nbsp;</span> Rebaixamento");
        }

        private void IdentificarCoresDestaqueBrasileiraoSerieB()
        {
            foreach (var time in classificacao)
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
            legenda.Add("<span class='badge bg-info'>&nbsp;</span> Promoção");
            legenda.Add("<span class='badge bg-danger'>&nbsp;</span> Rebaixamento");
        }
    }
}
