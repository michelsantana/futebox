using Futebox.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Pages.Shared
{
    public class BaseViewModel : PageModel
    {

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (Request.Query.ContainsKey("debug"))
            {
                var newmode = Request.Query["debug"];
                if (newmode == "1") Settings.DEBUGMODE = true;
                if (newmode == "0") Settings.DEBUGMODE = false;
            }
        }

        [HttpGet("toast")]
        public PartialViewResult OnGetToast(string title, string message)
        {
            return Partial("Templates/_toaster", Tuple.Create(title, message));
        }

        public bool UsarCache(PageViewModes viewMode) => viewMode == PageViewModes.padrao;
    }
}
