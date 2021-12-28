const axios = require('axios').default;
const Processo = require('./../model/processo');
const SubProcesso = require('./../model/subprocesso');

module.exports = class ProcessoServiceConstructor {
  api = '';
  constructor(api) {
    this.api = api;
  }

  /** @type {Processo} */
  async ObterProcesso() {
    return new Processo((await axios.get(`${this.api}/obter`)).data.processo);
  }

  /** @type {SubProcesso} */
  async ObterSubProcesso() {
    return new SubProcesso((await axios.get(`${this.api}/obter`)).data.subprocesso);
  }

  async AtualizarProcessoVideoCompleto(arquivo) {
    return new Processo((await axios.get(`${this.api}/videocompleto`).data));
  }

  async AtualizarProcessoVideoErro() {
    return new Processo((await axios.get(`${this.api}/videoerro`)).data);
  }

  async AtualizarProcessoPublicado(link) {
    return new Processo((await axios.get(`${this.api}/publicado?&link=${encodeURIComponent(link)}`)).data);
  }

  async AtualizarProcessoPublicacaoErro() {
    return new Processo((await axios.get(`${this.api}/publicacaoerro`)).data);
  }
};
