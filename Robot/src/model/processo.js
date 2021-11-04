const enumTipo = {
  [0]: "partida",
  [1]: "classificacao",
  [2]: "rodada",
};

const enumTipoLink = {
  [0]: "print",
  [1]: "image",
};

module.exports = function Processo(processo) {
  this.tipo = enumTipo[processo.tipo];
  this.idExterno = processo.idExterno;
  this.nome = processo.nome;
  this.link = processo.link;
  this.tipoLink = enumTipoLink[processo.tipoLink];
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

  return this;
};
