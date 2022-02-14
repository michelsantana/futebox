using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Base;
using Futebox.Services.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services.Jobs
{
    public class QueueJob : JobServiceProvider, IDisposable
    {

        public override async Task Execute(IJobExecutionContext context)
        {
            try
            {

                await base.Execute(context);
                await Execute(
                    context.JobDetail.Key,
                    context.MergedJobDataMap,
                    this.GetService<IQueueService>(),
                    this.GetService<IProcessoService>(),
                    this.GetService<INotifyService>()
                    );
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex.Message);
            }
        }

        public async static Task Execute(
            JobKey key,
            JobDataMap data,
            IQueueService _queueService,
            IProcessoService _processoService,
            INotifyService _notifyService)
        {
            try
            {
                var jobkey = key;
                Processo processo;
                List<SubProcesso> subProcessos;
                StringBuilder sbNotificacao = new StringBuilder();
                try
                {
                    processo = _processoService.ObterProcesso(data["processo"].ToString());
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
                        await _queueService.Executar(async () =>
                        {
                            var currentSub = sub;
                            if (!statusNaoExecutaveis.Any(_ => _ == currentSub.status))
                            {
                                if (statusGerarImagem.Any(_ => _ == currentSub.status))
                                {
                                    sbNotificacao.AppendLine(EyeLog.Log($"[IMAGEM][START][{jobkey.Name}]"));
                                    await _processoService.GerarImagem(processo, currentSub);
                                    sbNotificacao.AppendLine(EyeLog.Log($"[IMAGEM][COMPLETE][{jobkey.Name}]"));
                                }

                                if (statusGerarAudio.Any(_ => _ == currentSub.status))
                                {
                                    sbNotificacao.AppendLine(EyeLog.Log($"[AUDIO][START][{jobkey.Name}]"));
                                    await _processoService.GerarAudio(processo, currentSub);
                                    sbNotificacao.AppendLine(EyeLog.Log($"[AUDIO][COMPLETE][{jobkey.Name}]"));
                                }

                                if (statusGerarVideo.Any(_ => _ == currentSub.status))
                                {
                                    sbNotificacao.AppendLine(EyeLog.Log($"[VIDEO][START][{jobkey.Name}]"));
                                    await _processoService.GerarVideo(processo, currentSub);
                                    sbNotificacao.AppendLine(EyeLog.Log($"[VIDEO][COMPLETE][{jobkey.Name}]"));
                                }

                                if (statusPublicarVideo.Any(_ => _ == currentSub.status))
                                {
                                    sbNotificacao.AppendLine(EyeLog.Log($"[PUBLICAR][START][{jobkey.Name}]"));
                                    await _processoService.PublicarVideo(processo, currentSub);
                                    sbNotificacao.AppendLine(EyeLog.Log($"[PUBLICAR][COMPLETE][{jobkey.Name}]"));
                                }
                            }
                        });
                    }

                    sbNotificacao.AppendLine(EyeLog.Log($"[NOTIFY][START][{jobkey.Name}]"));
                    await _notifyService.Notify(processo.notificacao);
                    sbNotificacao.AppendLine(EyeLog.Log($"[NOTIFY][COMPLETE][{jobkey.Name}]"));
                }
                catch (Exception ex)
                {
                    sbNotificacao.AppendLine(EyeLog.Log($"[ERROR][{jobkey.Name}]"));
                    sbNotificacao.AppendLine(EyeLog.Log($"{ex.Message}"));
                    await _notifyService.Notify(sbNotificacao.ToString());
                }
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex.Message);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
