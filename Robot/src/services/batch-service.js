const fs = require('fs');
const child_process = require('child_process');
const Settings = require('../model/settings');
const ServiceResult = require('../model/serviceresult');
const Status = require('../model/statuscodes');
const utils = require('../utils/utils');

module.exports = class BatchService {
  /** @type {Settings} */
  settings;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  #ObterArquivoVideo() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.mp4`;
  }
  #ObterArquivoAtributos() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.attr.txt`;
  }
  #ObterShellAtributosTitulo() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.title.ps1`;
  }
  #ObterShellAtributosDescricao() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.description.ps1`;
  }
  #ObterShellFFMPEG() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.bat`;
  }

  GerarArquivosDeAtributosDoVideo() {
    const titulo = this.settings.processo.attrTitulo;
    const descricao = this.settings.processo.attrDescricao;
    const roteiro = this.settings.processo.roteiro;
    fs.writeFileSync(
      this.#ObterArquivoAtributos(),
      `[TITLE]
		\n\n${titulo}
		\n\n[DESCRIPTION]
		\n\n${descricao}
		\n\n[ROTEIRO]
		\n\n${roteiro.replace(/:/g, '\n')}`
    );

    fs.writeFileSync(this.#ObterShellAtributosTitulo(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${titulo}")`, { encoding: 'ascii' });
    fs.writeFileSync(this.#ObterShellAtributosDescricao(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${descricao}")`, { encoding: 'ascii' });
    return new ServiceResult(Status.ok, 'Arquivos gerados', `${this.#ObterArquivoAtributos()}|${this.#ObterShellAtributosTitulo()}|${this.#ObterShellAtributosDescricao()}`);
  }

  GerarBatchFFMPEG(arquivoImagem, arquivoAudio) {
    const conteudo = `ffmpeg -loop 1 -i ${arquivoImagem} -i ${arquivoAudio} -c:v libx264 -c:a copy -shortest ${this.#ObterArquivoVideo()}`;
    fs.writeFileSync(this.#ObterShellFFMPEG(), conteudo);
    return new ServiceResult(Status.ok, 'Arquivo gerado', `${this.#ObterShellFFMPEG()}`);
  }

  BatchGerarVideo() {
		fs.rmSync(this.#ObterArquivoVideo());
    child_process.execSync(`call ${this.#ObterShellFFMPEG()}`);
    return new ServiceResult(Status.ok, 'Vídeo gerado', `${this.#ObterArquivoVideo()}`);
  }

  BatchAbrirPasta() {
    child_process.execSync(`explorer.exe "${this.settings.pastaDestino}"`);
    return new ServiceResult(Status.ok, 'Pasta aberta');
  }
};
