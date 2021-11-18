const axios = require('axios').default;
const Processo = require('./../model/processomodel');

module.exports = class ProcessoServiceConstructor {
  datasourceApiUrl = '';
  constructor(datasourceApiUrl) {
    this.datasourceApiUrl = datasourceApiUrl;
  }

  /** @type {Processo} */
  async ObterProcesso() {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/obter`)).data);
  }

  async AtualizarProcessoErro(erro) {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/erro?mensagem=${erro}`)).data);
  }

  async AtualizarProcessoSucesso(arquivo) {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/sucesso?arquivo=${encodeURIComponent(arquivo)}`)).data);
  }
};
