using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Pages
{
    public class YoutubeModel : PageModel
    {
        readonly IFutebotService _futeBotService;
        public string Status = "⚪";
        public string Mensagem = "⚪";

        public YoutubeModel(IFutebotService futeBotService)
        {
            _futeBotService = futeBotService;
        }

        public void OnGet()
        {
            var result = _futeBotService.VerificarConfiguracaoYoutubeBrowser();
           
            switch (result.command)
            {
                case Models.Enums.RobotResultCommand.OK:
                    this.Status = "🟢";
                    this.Mensagem = "Credenciais do youtube estão configuradas!";
                    break;
                case Models.Enums.RobotResultCommand.ERROR:
                    this.Status = "🔴";
                    this.Mensagem = "Não foi possível obter o status das credencias por falha! " +
                        "Verifique se o perfil do Chromium está configurado.";
                    break;
                case Models.Enums.RobotResultCommand.AUTHFAILED:
                    this.Status = "🟡";
                    this.Mensagem = "Ação de login é necessária no perfil utilizado para upload!";
                    break;
                case Models.Enums.RobotResultCommand.BLANK:
                    this.Status = "⚪";
                    this.Mensagem = "⚪";
                    break;
                case Models.Enums.RobotResultCommand.INVALID:
                    this.Status = "⚪";
                    this.Mensagem = "⚪";
                    break;
                default:
                    break;
            }
        }
    }
}
