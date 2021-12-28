const fs = require('fs');
const child_process = require('child_process');
const BatchServiceSettings = require('../model/batch-service-settings');
const ServiceResult = require('../model/service-result');
const Status = require('../model/statuscodes');
const utils = require('../utils/utils');

module.exports = class BatchService {
  /** @type {BatchServiceSettings} */
  settings;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  #ObterArquivoImagem() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoImagem}`;
  }

  #ObterArquivoAudio() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoAudio}`;
  }

  #ObterArquivoVideo() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoVideo}`;
  }

  #ObterArquivoVideoParaConversao() {
    return `${this.settings.pasta}/temp.mp4`;
  }

  #ObterArquivoAtributos() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoTextoAtributos}`;
  }
  #ObterShellAtributosTitulo() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoShellAtributoTitulo}`;
  }
  #ObterShellAtributosDescricao() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoShellAtributoDescricao}`;
  }
  #ObterShellAtributosLegendaPostagem() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoShellAtributoLegendaPostagem}`;
  }
  #ObterShellFFMPEG() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivoShellFFMPEG}`;
  }

  GerarArquivosDeAtributosDoVideo() {
    const titulo = this.settings.titulo;
    const descricao = this.settings.descricao;
    const legenda = this.settings.legendaPostagem;

    if (titulo) fs.writeFileSync(this.#ObterShellAtributosTitulo(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${titulo}")`, { encoding: 'ascii' });
    if (descricao) fs.writeFileSync(this.#ObterShellAtributosDescricao(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${descricao}")`, { encoding: 'ascii' });
    if (legenda) fs.writeFileSync(this.#ObterShellAtributosLegendaPostagem(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${legenda}")`, { encoding: 'ascii' });

    return new ServiceResult(Status.ok, 'Arquivos gerados', `${this.#ObterArquivoAtributos()}|${this.#ObterShellAtributosTitulo()}|${this.#ObterShellAtributosDescricao()}`);
  }

  GerarBatchFFMPEG() {
    let conteudo = '';
    if (this.settings.usarConversaoIG) {
      conteudo = `
      ffmpeg -loop 1 -i ${this.#ObterArquivoImagem()} -i ${this.#ObterArquivoAudio()} -c:v libx264 -c:a copy -shortest ${this.#ObterArquivoVideoParaConversao()}
      ffmpeg -i ${this.#ObterArquivoVideoParaConversao()} -c:a aac -b:a 256k -ar 44100 -c:v libx264 -b:v 5M -r 30 -pix_fmt yuv420p -preset faster -tune stillimage ${this.#ObterArquivoVideo()}`;
    } else {
      conteudo = `
      ffmpeg -loop 1 -i ${this.#ObterArquivoImagem()} -i ${this.#ObterArquivoAudio()} -c:v libx264 -c:a copy -shortest ${this.#ObterArquivoVideo()}`;
    }

    fs.writeFileSync(this.#ObterShellFFMPEG(), conteudo);
    return new ServiceResult(Status.ok, 'Arquivo gerado', `${this.#ObterShellFFMPEG()}`);
  }

  BatchGerarVideo() {
    if (utils.existeArquivo(this.#ObterArquivoVideo())) fs.rmSync(this.#ObterArquivoVideo());
    child_process.execSync(`call ${this.#ObterShellFFMPEG()}`);
    return new ServiceResult(Status.ok, 'Vídeo gerado', `${this.#ObterArquivoVideo()}`);
  }

  static BatchAbrirPasta(pasta) {
    child_process.exec(`explorer.exe "${pasta}"`);
    return new ServiceResult(Status.ok, 'Pasta aberta');
  }
};

// ffmpeg -i a.mp4 -c:a aac -b:a 256k -ar 44100 -c:v libx264 -b:v 5M -r 30 -pix_fmt yuv420p -preset faster -tune stillimage b.mp4
// ffmpeg -loop 1 -i a.png -i a.mp3  -c:v libx264 -b:v 5M -c:a aac -r 30 -pix_fmt -shortest a.mp4
// ffmpeg -loop 1 -i a.png -i a.mp3 -c:v libx264 -b:v 5M -c:a aac -r 30 -pix_fmt yuv420p -preset faster -tune stillimage a.mp4
