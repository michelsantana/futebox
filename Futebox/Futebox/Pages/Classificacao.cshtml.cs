using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Futebox.Models.Enums;

namespace Futebox.Pages
{
    public class ClassificacaoModel : PageModel
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

        public void OnGet(string foco, string printMode)
        {
            this.clearCache = !string.IsNullOrEmpty(printMode);
            classificacao = new List<ClassificacaoVM>();
            if (!string.IsNullOrEmpty(foco))
                this.campeonatoFoco = ((Campeonatos)int.Parse(foco));
        }

        public PartialViewResult OnGetCampeonato(int campeonato)
        {
            var enumCampeonato = (Campeonatos)campeonato;
            switch (enumCampeonato)
            {
                case Campeonatos.BrasileiraoSerieA:
                    nomeCampeonato = "Brasileirão Série A";
                    this.classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(Campeonatos.BrasileiraoSerieA, clearCache)?.ToList();
                    IdentificarCoresDestaqueBrasileiraoSerieA();
                    break;
                case Campeonatos.BrasileiraoSerieB:
                    nomeCampeonato = "Brasileirão Série B";
                    this.classificacao = _classificacaoService.ObterClassificacaoPorCampeonato(Campeonatos.BrasileiraoSerieB, clearCache)?.ToList();
                    IdentificarCoresDestaqueBrasileiraoSerieB();
                    break;
            }
            return Partial("Templates/_tabelaDeClassificacao", this);
        }

        private void IdentificarCoresDestaqueBrasileiraoSerieA()
        {
            legenda.Clear();
            foreach (var time in this.classificacao)
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
            legenda.Clear();
            foreach (var time in this.classificacao)
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
