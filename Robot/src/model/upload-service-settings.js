module.exports = class UploadServiceSettings {
  pasta = '';
  nomeDoArquivoVideo = '';
  redeSocial = '';
  legendaPostagem = '';
  titulo = '';
  descricao = '';
  playlist = '';

  RedeSocialEnum = {
    [1]: 'YoutubeShorts',
    [2]: 'YoutubeVideo',
    [3]: 'InstagramVideo',
  }

  constructor({ pastaDoArquivo, nomeDoArquivoVideo, redeSocial, legendaPostagem, tituloVideo, descricaoVideo, playlist }) {
    this.pasta = pastaDoArquivo;
    this.nomeDoArquivoVideo = nomeDoArquivoVideo;
    this.redeSocial = this.RedeSocialEnum[redeSocial];
    this.legendaPostagem = legendaPostagem;
    this.titulo = tituloVideo;
    this.descricao = descricaoVideo;
    this.playlist = playlist;
  }
};
