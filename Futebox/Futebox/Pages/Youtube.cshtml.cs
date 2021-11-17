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

        public YoutubeModel(IFutebotService futeBotService)
        {
            _futeBotService = futeBotService;
        }

        public void OnGet(string status)
        {
            _futeBotService.VerificarConfiguracaoYoutubeBrowser();
        }
    }
}
