using Futebox.Pages.Shared;
using Microsoft.Extensions.Logging;

namespace Futebox.Pages
{
    public class IndexModel : BaseViewModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
