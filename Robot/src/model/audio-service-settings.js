const SubProcesso = require("./subprocesso");

module.exports = class AudioServiceSettings {
  nomeDoArquivo = '';
  pasta = '';
  roteiro = '';
  usarArquivosExistentes = true;

  constructor({ nomeDoArquivoAudio, pastaDoArquivo, roteiro }) {
    this.nomeDoArquivo = nomeDoArquivoAudio;
    this.pasta = pastaDoArquivo;
    this.roteiro = roteiro;
    this.usarArquivosExistentes = true;
  }
};
