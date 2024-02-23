const path = require('path');
module.exports = class ImagemServiceSettings {
  link = '';
  pasta = '';
  nomeDoArquivo = '';
  largura = 0;
  altura = 0;
  usarArquivosExistentes = true;
  constructor({ linkDaImagemDoVideo, pastaDoArquivo, nomeDoArquivoImagem, larguraVideo, alturaVideo }) {
    this.link = linkDaImagemDoVideo;
    this.pasta = path.resolve(pastaDoArquivo);
    this.nomeDoArquivo = nomeDoArquivoImagem;
    this.largura = larguraVideo;
    this.altura = alturaVideo;
  }
};
