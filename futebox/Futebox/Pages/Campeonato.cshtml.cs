using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Futebox.Pages
{
    public class CampeonatoModel : PageModel
    {
        public List<FootstatsTime> api { get; set; }
        public List<Time> db { get; set; }

        IGerenciamentoTimesService _gerenciamentoTimesService;
        ICacheHandler _cacheHandler;

        public CampeonatoModel(IGerenciamentoTimesService gerenciamentoTimesService, ICacheHandler cacheHandler)
        {
            _gerenciamentoTimesService = gerenciamentoTimesService;
            _cacheHandler = cacheHandler;
        }

        public void OnGet()
        {
            CarregarApi();
            CarregarDB();
        }

        public PartialViewResult OnGetTbApi()
        {
            CarregarApi();
            return Partial("Templates/_tabelaDosTimesDaApi", this);
        }

        public PartialViewResult OnGetTbDb()
        {
            CarregarDB();
            return Partial("Templates/_tabelaDosTimesDoBanco", this);
        }
        private void CarregarApi()
        {
            var resultado = _cacheHandler.ObterConteudo<List<FootstatsTime>>(nameof(FootstatsTime));
            if (resultado == null)
            {
                resultado = _gerenciamentoTimesService.ObterTimesServico();
                _cacheHandler.DefinirConteudo(nameof(FootstatsTime), resultado, 48);
            }
            resultado.ForEach(_ => TentarCarregarLogoLocal(_));
            this.api = resultado;
        }

        private void CarregarDB()
        {
            this.db = _gerenciamentoTimesService.ObterTimesBancoDados();
        }

        private void TentarCarregarLogoLocal(FootstatsTime _)
        {
            _.urlLogo = $"/img/Logo_{_.sigla.ToUpper()}.png";
        }
    }

}
