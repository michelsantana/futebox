using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Models.Viewmodels;
using Futebox.Pages.Shared;
using Futebox.Services.Interfaces;
using Futebox.Services.utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Pages
{
    public class ProcessosModel : BaseViewModel
    {
        IProcessoService _processoService;
        IAgendamentoService _agendamentoService;

        public List<Processo> processos = new List<Processo>();

        public ProcessosModel(IProcessoService processoService, IAgendamentoService agendamentoService)
        {
            _processoService = processoService;
            _agendamentoService = agendamentoService;
        }

        public void OnGet()
        {
            Promise.All(
                async () => {
                    this.processos = (await _processoService.ObterProcessos())?.ToList();
                    this.processos = this.processos.OrderBy(_ => (_.criacao.Ticks) * -1).ToList();
                }
            );
        }

        //public PartialViewResult OnGetTableAgenda(string processoId)
        //{
        //    //var agendas = new List<Agenda>();
        //    //var running = new List<IJobExecutionContext>();
        //    //var jobs = new List<IJobDetail>();
        //    //Promise.All(
        //    //    async () => agendas = await _agendamentoService.List(),
        //    //    async () => jobs = await _agendamentoService.JobList(),
        //    //    async () => running = await _agendamentoService.JobListRunning());

        //    //var vm = agendas.Select(_ => {
        //    //    var avm = new AgendaVM();
        //    //    avm = JsonConvert.DeserializeObject<AgendaVM>(JsonConvert.SerializeObject(_));
        //    //    Promise.All(() => avm.hasJob = jobs.Any(_ => _.Key.Name == avm.processoId));
        //    //    return avm;
        //    //});

        //    //return Partial("Templates/_processoTableAgenda", agendas));
        //}
    }
}
