module.exports = class BatchServiceSettings {
  pasta = '';
  nomeDoArquivoImagem = '';
  nomeDoArquivoAudio = '';
  nomeDoArquivoVideo = '';

  nomeDoArquivoShellAtributoTitulo = '';
  nomeDoArquivoShellAtributoDescricao = '';
  nomeDoArquivoShellAtributoLegendaPostagem = '';
  nomeDoArquivoShellFFMPEG = '';

  titulo = '';
  descricao = '';
  legendaPostagem = '';
  roteiro = '';
  usarConversaoIG = '';
  userArquivosExistentes = false;

  RedeSocialEnum = {
    [1]: 'YoutubeShorts',
    [2]: 'YoutubeVideo',
    [3]: 'InstagramVideo',
  }

  constructor({ pastaDoArquivo, tituloVideo, descricaoVideo, redeSocial, roteiro, nomeDoArquivoImagem, nomeDoArquivoAudio, nomeDoArquivoVideo }) {

    this.titulo = tituloVideo;
    this.descricao = descricaoVideo;
    this.roteiro = roteiro;

    this.pasta = pastaDoArquivo;
    this.nomeDoArquivoImagem = nomeDoArquivoImagem;
    this.nomeDoArquivoAudio = nomeDoArquivoAudio;
    this.nomeDoArquivoVideo = nomeDoArquivoVideo;

    this.redeSocial = this.RedeSocialEnum[redeSocial];

    this.nomeDoArquivoShellAtributoTitulo = `${this.RedeSocialEnum[redeSocial]}.titulo.ps1`;
    this.nomeDoArquivoShellAtributoDescricao = `${this.RedeSocialEnum[redeSocial]}.descricao.ps1`;
    this.nomeDoArquivoShellAtributoLegendaPostagem = `${this.RedeSocialEnum[redeSocial]}.legenda.ps1`;
    this.nomeDoArquivoShellFFMPEG = `${this.RedeSocialEnum[redeSocial]}.bat`;

    if(this.redeSocial == this.RedeSocialEnum[3]) this.usarConversaoIG = true;
  }
};
