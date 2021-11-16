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
        readonly IYoutubeService _youtubeService;

        public YoutubeModel(IYoutubeService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        public void OnGet(string status)
        {
            if (status == "login") _youtubeService.DoLogin();
            if (status == "logout") _youtubeService.DoLogout();
        }
    }
}
