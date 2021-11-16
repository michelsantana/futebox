module.exports = {
  ServicoFala: function ServicoFala(status, mensagem, arquivoDestino) {
    this.status = status;
    this.mensagem = mensagem;
    this.arquivoDestino = arquivoDestino;
    return this;
  },
  ServicoUpload: function ServicoUpload(status, mensagem) {
      
  },
};
