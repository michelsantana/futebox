module.exports = {
  ServicoFalaRetorno: function ServicoFala(status, mensagem, arquivoDestino) {
    this.status = status;
    this.mensagem = mensagem;
    this.arquivoDestino = arquivoDestino;
    return this;
  },
  ServicoFalaSettings: function ServicoFalaSettings(pasta, arquivo, roteiro, usarArquivosExistentes) {
    this.pasta = pasta;
    this.arquivo = arquivo
    this.roteiro = roteiro;
    this.usarArquivosExistentes = usarArquivosExistentes;
    return this;
  },
  ServiceResult: function ServiceResult(status, mensagem){
    this.status = status;
    this.mensagem = mensagem;
    return this;
  }
};
