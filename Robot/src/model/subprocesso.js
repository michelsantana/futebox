module.exports = class SubProcesso {
  id = null;
  criacao = null;
  alteracao = null;
  alturaVideo = null;
  categoriaVideo = null;
  descricaoVideo = null;
  larguraVideo = null;
  linkDaImagemDoVideo = null;
  linkPostagem = null;
  nomeDoArquivoAudio = null;
  nomeDoArquivoImagem = null;
  nomeDoArquivoVideo = null;
  pastaDoArquivo = null;
  playlist = null;
  processoId = null;
  redeSocial = null;
  roteiro = null;
  status = null;
  tituloVideo = null;
  legendaPostagem = null;

  constructor({
    id,
    criacao,
    alteracao,
    alturaVideo,
    categoriaVideo,
    descricaoVideo,
    larguraVideo,
    linkDaImagemDoVideo,
    linkPostagem,
    nomeDoArquivoAudio,
    nomeDoArquivoImagem,
    nomeDoArquivoVideo,
    pastaDoArquivo,
    playlist,
    processoId,
    redeSocial,
    roteiro,
    status,
    tituloVideo,
    legendaPostagem,
  }) {
    this.id = id;
    this.criacao = criacao;
    this.alteracao = alteracao;
    this.alturaVideo = alturaVideo;
    this.categoriaVideo = categoriaVideo;
    this.descricaoVideo = descricaoVideo;
    this.larguraVideo = larguraVideo;
    this.linkDaImagemDoVideo = linkDaImagemDoVideo;
    this.linkPostagem = linkPostagem;
    this.nomeDoArquivoAudio = nomeDoArquivoAudio;
    this.nomeDoArquivoImagem = nomeDoArquivoImagem;
    this.nomeDoArquivoVideo = nomeDoArquivoVideo;
    this.pastaDoArquivo = pastaDoArquivo;
    this.playlist = playlist;
    this.processoId = processoId;
    this.redeSocial = redeSocial;
    this.roteiro = roteiro;
    this.status = status;
    this.tituloVideo = tituloVideo;
    this.legendaPostagem = legendaPostagem;
  }
};
