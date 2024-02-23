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

        public List<Tuple<Processo, Agenda>> processos = new List<Tuple<Processo, Agenda>>();

        public ProcessosModel(IProcessoService processoService, IAgendamentoService agendamentoService)
        {
            _processoService = processoService;
            _agendamentoService = agendamentoService;
        }

        public void OnGet()
        {
            var p = new List<Processo>();
            var a = new List<Agenda>();
            Task.WaitAll(Promise.All(
                async () =>
                {
                    p = (await _processoService.ObterProcessos())?.ToList();
                    p = p.OrderBy(_ => (_.criacao.Ticks) * -1).ToList();
                },
                async () =>
                {
                    a = (await _agendamentoService.List())?.ToList();
                    a = a.OrderBy(_ => (_.criacao.Ticks) * -1).ToList();
                }
            ));
            processos = p.Select(s => Tuple.Create(s, a.Find(_ => _.processoId == s.id))).ToList();
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
