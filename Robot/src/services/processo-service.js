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

  async AtualizarProcessoVideoCompleto(arquivo) {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/videocompleto?arquivo=${encodeURIComponent(arquivo)}`)).data);
  }

  async AtualizarProcessoVideoErro() {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/videoerro`)).data);
  }

  async AtualizarProcessoPublicado(link) {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/publicado?link=${encodeURIComponent(link)}`)).data);
  }

  async AtualizarProcessoPublicacaoErro() {
    return new Processo((await axios.get(`${this.datasourceApiUrl}/publicacaoerro`)).data);
  }
};
