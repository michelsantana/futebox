const { TipoProcesso, TipoLink } = require('./enums');

module.exports = class Processo {
  tipo = new TipoProcesso();
  idExterno = null;
  nome = null;
  link = null;
  tipoLink = new TipoLink();
  imgLargura = null;
  imgAltura = null;
  roteiro = null;
  attrTitulo = null;
  attrDescricao = null;
  status = null;
  processado = null;
  json = null;
  linkThumb = null;
  criacao = null;
  notificacao = null;
  arquivoVideo = null;
  portaExecucao = null;
  
  constructor(processo) {
    this.tipo = new TipoProcesso(processo.tipo);
    this.idExterno = processo.idExterno;
    this.nome = processo.nome;
    this.link = processo.link;
    this.tipoLink = new TipoLink(processo.tipoLink);
    this.imgLargura = processo.imgLargura;
    this.imgAltura = processo.imgAltura;
    this.roteiro = processo.roteiro;
    this.attrTitulo = processo.attrTitulo;
    this.attrDescricao = processo.attrDescricao;
    this.status = processo.status;
    this.processado = processo.processado;
    this.json = processo.json;
    this.linkThumb = processo.linkThumb;
    this.criacao = processo.criacao;
    this.notificacao = processo.notificacao;
    this.arquivoVideo = processo.arquivoVideo;
    this.portaExecucao = processo.portaExecucao;
  }
};
