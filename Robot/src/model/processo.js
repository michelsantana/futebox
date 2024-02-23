module.exports = class Processo {
  nome = null;
  categoria = null;
  status = null;
  statusMensagem = null;
  notificacao = null;
  agendamento = null;
  agendado = null;
  partidaId = null;
  campeonatoId = null;
  rodadaId = null;
  

  constructor(processo) {
    this.nome = processo.nome;
    this.categoria = processo.categoria;
    this.status = processo.status;
    this.statusMensagem = processo.statusMensagem;
    this.notificacao = processo.notificacao;
    this.agendamento = processo.agendamento;
    this.agendado = processo.agendado;
    this.partidaId = processo.partidaId;
    this.campeonatoId = processo.campeonatoId;
    this.rodadaId = processo.rodadaId;
  }
};
