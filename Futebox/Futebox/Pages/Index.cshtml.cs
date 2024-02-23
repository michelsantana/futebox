using Futebox.Pages.Shared;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Futebox.Pages
{
    public class IndexModel : BaseViewModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBrowserService _browser;
        private readonly IAgendamentoService _agenda;

        public IndexModel(ILogger<IndexModel> logger, IBrowserService browser, IAgendamentoService agenda)
        {
            _logger = logger;
            _browser = browser;
            _agenda = agenda;
        }

        public void OnGet()
        {
            if (!Settings.ScheduleInitialized)
            {
                _agenda.InitializeAllJobs();
                Settings.ScheduleInitialized = true;
            }
        }
    }
}
