const Processo = require('./../model/processomodel');
const utils = require('./../utils/utils');

module.exports = class Settings {
  pastaDestino = null;
  nomeArquivoDestino = null;
  /** @type {Processo} */
  processo;
  usarArquivosExistentes = null;
  constructor(pastaDestino, nomeArquivoDestino, processo, usarArquivosExistentes) {
    this.pastaDestino = pastaDestino;
    this.nomeArquivoDestino = nomeArquivoDestino;
    this.processo = processo;
    this.usarArquivosExistentes = usarArquivosExistentes;
    if(this.pastaDestino) utils.criarPastaSeNaoExistir(pastaDestino);
  }
};
