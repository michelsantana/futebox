using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Futebox.Pages
{
    public class SocialMedias : PageModel
    {
        readonly IFutebotService _futeBotService;
        public string YT = "Youtube - ⚪";
        public string IG = "Instagram - ⚪";

        public SocialMedias(IFutebotService futeBotService)
        {
            _futeBotService = futeBotService;
        }

        public void OnGet()
        {
            var yt = _futeBotService.VerificarConfiguracaoYoutubeBrowser();
            var ig = _futeBotService.VerificarConfiguracaoInstagramBrowser();

            switch (yt.status)
            {
                case HttpStatusCode.OK:
                    this.YT = "Youtube - 🟢 \n";
                    this.YT += "Credenciais configuradas!";
                    break;
                case HttpStatusCode.InternalServerError:
                    this.YT = "Youtube - 🔴 \n";
                    this.YT += "Não foi possível obter o status das credencias por falha! " +
                        "Verifique se o perfil do Canary está configurado.";
                    break;
                case HttpStatusCode.Unauthorized:
                    this.YT = "Youtube - 🟡 \n";
                    this.YT += "Ação de login é necessária no perfil utilizado para upload!";
                    break;
                case HttpStatusCode.NotFound:
                    this.YT = "Youtube - ⚪ \n";
                    this.YT += "Resultado inesperado";
                    break;
                case HttpStatusCode.BadRequest:
                    this.YT = "Youtube - ⚪ \n";
                    this.YT += "Resultado inválido";
                    break;
                default:
                    break;
            }

            switch (ig.status)
            {
                case HttpStatusCode.OK:
                    this.IG = "Instagram - 🟢 \n";
                    this.IG += "Credenciais configuradas!";
                    break;
                case HttpStatusCode.InternalServerError:
                    this.IG = "Instagram - 🔴 \n";
                    this.IG += "Não foi possível obter o status das credencias por falha! " +
                        "Verifique se o perfil do Canary está configurado.";
                    break;
                case HttpStatusCode.Unauthorized:
                    this.IG = "Instagram - 🟡 \n";
                    this.IG += "Ação de login é necessária no perfil utilizado para upload!";
                    break;
                case HttpStatusCode.NotFound:
                    this.IG = "Instagram - ⚪ \n";
                    this.IG += "Resultado inesperado";
                    break;
                case HttpStatusCode.BadRequest:
                    this.IG = "Instagram - ⚪ \n";
                    this.IG += "Resultado inválido";
                    break;
                default:
                    break;
            }
        }
    }
}
