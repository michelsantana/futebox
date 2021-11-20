const pptr = require('puppeteer');
const { Browser, Page } = require('puppeteer');

const Settings = require('../model/settings');
const ServiceResult = require('../model/serviceresult');
const Status = require('../model/statuscodes');
const utils = require('./../utils/utils');

module.exports = class ImagemService {
  /** @type {Settings} */
  settings;
  /** @type {Browser} */
  browser;
  /** @type {Page} */
  page;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  #ObterImagemVideo() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.png`;
  }
  #ObterThumbVideo() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.thumb.png`;
  }

  #ExisteArquivoImagemVideo() {
    if (this.settings.usarArquivosExistentes && utils.existeArquivo(this.#ObterImagemVideo())) return true;

    return false;
  }

  #ExisteArquivoThumbVideo() {
    if (!this.settings.processo.linkThumb) return true;
    if (this.settings.usarArquivosExistentes && utils.existeArquivo(this.#ObterThumbVideo())) return true;
    return false;
  }

  async #Esperar(timeoutMs) {
    await utils.sleep(timeoutMs);
  }

  async #AbrirBrowser() {
    this.browser = await pptr.launch({
      headless: false,
      args: ['--use-fake-ui-for-media-stream'],
      ignoreDefaultArgs: ['--mute-audio'],
    });
  }

  async #FecharBrowser() {
    await this.browser?.close();
  }

  async #AbrirPagina() {
    this.page = await this.browser.newPage();
    await this.page.setViewport({
      width: ~~this.settings.processo.imgLargura,
      height: ~~this.settings.processo.imgAltura,
    });
  }

  async #NavegarParaImagem() {
    await this.page.goto(this.settings.processo.link, { waitUntil: 'networkidle2' });
    await this.#Esperar(8);
  }

  async #NavegarParaThumb() {
    await this.page.goto(this.settings.processo.link, { waitUntil: 'networkidle2' });
    await this.#Esperar(8);
  }

  async #SalvarPrint(salvar) {
    await this.page.screenshot({ path: salvar });
  }

  async Executar() {
    try {
      const arquivoImagemCache = this.#ExisteArquivoImagemVideo();
      //const arquivoThumbCache = this.#ExisteArquivoThumbVideo();

      if (arquivoImagemCache 
        //&& arquivoThumbCache
        ) return new ServiceResult(Status.ok, 'Arquivo gerado com sucesso', `${this.#ObterImagemVideo()}|${this.#ObterThumbVideo()}`);

      await this.#AbrirBrowser();
      await this.#AbrirPagina();

      if (!arquivoImagemCache) {
        await this.#NavegarParaImagem();
        await this.#SalvarPrint(this.#ObterImagemVideo());
      }

      // if (!arquivoThumbCache) {
      //   await this.#NavegarParaThumb();
      //   await this.#SalvarPrint(this.#ObterThumbVideo());
      // }

      await this.#FecharBrowser();
      return new ServiceResult(Status.ok, 'Arquivo gerado com sucesso', `${this.#ObterImagemVideo()}|${this.#ObterThumbVideo()}`);
    } catch (ex) {
      this.#FecharBrowser();
      throw ex;
    }
  }
};
