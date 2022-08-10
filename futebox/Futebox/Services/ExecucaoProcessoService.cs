using Futebox.Models;
using Futebox.Models.Enums;
using Futebox.Services.Interfaces;
using Futebox.Services.utils;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class ExecucaoProcessoService : IExecucaoProcessoService
    {
        IProcessoService _processoService;
        IAgendamentoService _agendamentoService;
        IQueueService _queueService;
        INotifyService _notifyService;

        class StatusSteps
        {
            static StatusProcesso[] Group(params StatusProcesso[] s)
            {
                return s;
            }
            public static StatusProcesso[] statusNaoExecutaveis
                = Group(StatusProcesso.Criado, StatusProcesso.PublicandoOK);

            public static StatusProcesso[] statusGerarImagem
                = Group(StatusProcesso.Agendado, StatusProcesso.ImagemErro);

            public static StatusProcesso[] statusGerarAudio
                = Group(StatusProcesso.ImagemOK, StatusProcesso.AudioErro);

            public static StatusProcesso[] statusGerarVideo
                = Group(StatusProcesso.AudioOK, StatusProcesso.VideoErro);

            public static StatusProcesso[] statusPublicarVideo
                = Group(StatusProcesso.VideoOK, StatusProcesso.PublicandoErro);
        }

        public ExecucaoProcessoService(
            IProcessoService processoService,
            IQueueService queueService,
            INotifyService notifyService,
            IAgendamentoService agendamentoService)
        {
            _processoService = processoService;
            _queueService = queueService;
            _notifyService = notifyService;
            _agendamentoService = agendamentoService;
        }

        public async Task<RobotResultApi> Executar(string processoId)
        {
            RobotResultApi result = new RobotResultApi($"ExecucaoProcessoService.{processoId}");
            await _queueService.Executar(async () =>
            {
                var log = new Action<string>((m) => result.Add(m));
                log("Iniciando");

                Processo processo = null;
                Agenda agenda = null;
                StringBuilder sbNotificacao = new StringBuilder();
                try
                {
                    result.Add("Buscar processo");
                    Promise.All(async () => processo = await _processoService.Obter(processoId),
                        async () =>
                        {
                            agenda = await _agendamentoService.Obter(processoId);
                            agenda.status = Agenda.Status.executando;
                            agenda = await _agendamentoService.Salvar(agenda);
                        }
                    );

                    if (!StatusSteps.statusNaoExecutaveis.Any(_ => _ == processo.status))
                    {
                        if (StatusSteps.statusGerarImagem.Any(_ => _ == processo.status))
                        {
                            log(EyeLog.Log($"[IMAGEM][START][{processoId}]"));
                            await _processoService.GerarImagem(processo);
                            log(EyeLog.Log($"[IMAGEM][COMPLETE][{processoId}]"));
                        }

                        if (StatusSteps.statusGerarAudio.Any(_ => _ == processo.status))
                        {
                            log(EyeLog.Log($"[AUDIO][START][{processoId}]"));
                            await _processoService.GerarAudio(processo);
                            log(EyeLog.Log($"[AUDIO][COMPLETE][{processoId}]"));
                        }

                        if (StatusSteps.statusGerarVideo.Any(_ => _ == processo.status))
                        {
                            log(EyeLog.Log($"[VIDEO][START][{processoId}]"));
                            await _processoService.GerarVideo(processo);
                            log(EyeLog.Log($"[VIDEO][COMPLETE][{processoId}]"));
                        }

                        if (StatusSteps.statusPublicarVideo.Any(_ => _ == processo.status))
                        {
                            log(EyeLog.Log($"[PUBLICAR][START][{processoId}]"));
                            await _processoService.PublicarVideo(processo);
                            log(EyeLog.Log($"[PUBLICAR][COMPLETE][{processoId}]"));
                        }
                    }

                    agenda.status = Agenda.Status.concluido;
                    Promise.All(
                        () => _notifyService.Notify(processo.nome),
                        async () => agenda = await _agendamentoService.Salvar(agenda)
                    );
                    result.Ok();
                }
                catch (Exception ex)
                {
                    log(EyeLog.Log($"[ERROR][{processoId}]"));
                    log(EyeLog.Log($"{ex.Message}"));
                    agenda.status = Agenda.Status.erro;
                    Promise.All(() => _agendamentoService.Salvar(agenda));
                    result.Error();
                }
            });
            return result;
        }
    }
}
