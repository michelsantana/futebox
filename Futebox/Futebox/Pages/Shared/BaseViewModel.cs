using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Pages.Shared
{
    public class BaseViewModel : PageModel
    {
        [HttpGet("toast")]
        public PartialViewResult OnGetToast(string title, string message)
        {
            return Partial("Templates/_toaster", Tuple.Create(title, message));
        }

    }
}
