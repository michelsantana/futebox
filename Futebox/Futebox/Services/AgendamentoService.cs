using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class AgendamentoService : IAgendamentoService
    {

        IProcessoService _processoService;
        INotifyService _notifyService;
        SchedulerService _schedule;

        [DisallowConcurrentExecution]
        public class ExecutarProcessoJob : IJob
        {

            public Task Execute(IJobExecutionContext context)
            {
                var jobkey = context.JobDetail.Key;
                IProcessoService _processoService;
                INotifyService _notifyService = null;
                Processo processo;
                List<SubProcesso> subProcessos;
                StringBuilder sbNotificacao = new StringBuilder();
                try
                {
                    _processoService = (IProcessoService)context.MergedJobDataMap[nameof(IProcessoService)];
                    _notifyService = (INotifyService)context.MergedJobDataMap[nameof(INotifyService)];
                    processo = _processoService.ObterProcesso(context.MergedJobDataMap["processo"].ToString());
                    subProcessos = _processoService.ObterSubProcessos(processo.id);
                    var statusNaoExecutaveis = new StatusProcesso[]
                    {
                        StatusProcesso.Criado, StatusProcesso.PublicandoOK,
                    };

                    var statusGerarImagem = new StatusProcesso[]
                    {
                        StatusProcesso.Agendado, StatusProcesso.ImagemErro
                    };
                    var statusGerarAudio = new StatusProcesso[]
                    {
                        StatusProcesso.ImagemOK, StatusProcesso.AudioErro
                    };
                    var statusGerarVideo = new StatusProcesso[]
                    {
                        StatusProcesso.AudioOK, StatusProcesso.VideoErro
                    };
                    var statusPublicarVideo = new StatusProcesso[]
                    {
                        StatusProcesso.VideoOK, StatusProcesso.PublicandoErro
                    };
                    foreach (var sub in subProcessos)
                    {
                        var currentSub = sub;
                        if (!statusNaoExecutaveis.Any(_ => _ == currentSub.status))
                        {

                            if (statusGerarImagem.Any(_ => _ == currentSub.status))
                            {
                                sbNotificacao.AppendLine(EyeLog.Log($"[IMAGEM][START][{jobkey.Name}]"));
                                _processoService.GerarImagem(ref processo, ref currentSub);
                                sbNotificacao.AppendLine(EyeLog.Log($"[IMAGEM][COMPLETE][{jobkey.Name}]"));
                            }
                            
                            if (statusGerarAudio.Any(_ => _ == currentSub.status))
                            {
                                sbNotificacao.AppendLine(EyeLog.Log($"[AUDIO][START][{jobkey.Name}]"));
                                _processoService.GerarAudio(ref processo, ref currentSub);
                                sbNotificacao.AppendLine(EyeLog.Log($"[AUDIO][COMPLETE][{jobkey.Name}]"));
                            }

                            if (statusGerarVideo.Any(_ => _ == currentSub.status))
                            {
                                sbNotificacao.AppendLine(EyeLog.Log($"[VIDEO][START][{jobkey.Name}]"));
                                _processoService.GerarVideo(ref processo, ref currentSub);
                                sbNotificacao.AppendLine(EyeLog.Log($"[VIDEO][COMPLETE][{jobkey.Name}]"));
                            }

                            if (statusPublicarVideo.Any(_ => _ == currentSub.status))
                            {
                                sbNotificacao.AppendLine(EyeLog.Log($"[PUBLICAR][START][{jobkey.Name}]"));
                                _processoService.PublicarVideo(ref processo, ref currentSub);
                                sbNotificacao.AppendLine(EyeLog.Log($"[PUBLICAR][COMPLETE][{jobkey.Name}]"));
                            }
                        }
                    }

                    sbNotificacao.AppendLine(EyeLog.Log($"[NOTIFY][START][{jobkey.Name}]"));
                    _notifyService.Notify(processo.notificacao);
                    sbNotificacao.AppendLine(EyeLog.Log($"[NOTIFY][COMPLETE][{jobkey.Name}]"));
                }
                catch (Exception ex)
                {
                    sbNotificacao.AppendLine(EyeLog.Log($"[ERROR][{jobkey.Name}]"));
                    sbNotificacao.AppendLine(EyeLog.Log($"{ex.Message}"));
                    _notifyService?.Notify(sbNotificacao.ToString());
                }
                finally
                {
                    Task.WaitAll(context.Scheduler.DeleteJob(jobkey));
                }
                return Task.FromResult(true);
            }
        }

        public AgendamentoService(IProcessoService processoService, INotifyService notifyService, IServiceProvider provider, SchedulerService schedule)
        {
            _processoService = processoService;
            _notifyService = notifyService;
            _schedule = schedule;
        }

        public void AgendarExecucao(string processoId, DateTime date)
        {

            var jobExecuteId = JobKey.Create($"execucao-processo-{processoId}", "geral");

            //AddJob<PublicarVideoJob>(processoId, jobNotifyId, _processoService, date);

            AddJob<ExecutarProcessoJob>(processoId, jobExecuteId, date);
        }

        private void AddJob<T>(string processoId, JobKey jobId, DateTime date) where T : IJob
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("processo", processoId);
            dict.Add(nameof(INotifyService), _notifyService);
            dict.Add(nameof(IProcessoService), _processoService);
            dict.Add("jobid", jobId);

            EyeLog.Log($"Agendando processo - {processoId}");

            IJobDetail job = JobBuilder.Create<T>()
                .WithIdentity(jobId)
                .StoreDurably(false)
                .SetJobData(new JobDataMap(dict))
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(jobId.Name, jobId.Group)
                .StartAt(date)
                .Build();

            _schedule.AddJob(job, trigger);
        }
    }
}
