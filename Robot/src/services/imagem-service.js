const pptr = require('puppeteer');
const { Browser, Page } = require('puppeteer');
const ImagemServiceSettings = require('../model/imagem-service-settings');

const ServiceResult = require('../model/service-result');
const Status = require('../model/statuscodes');
const utils = require('./../utils/utils');

module.exports = class ImagemService {
  /** @type {ImagemServiceSettings} */
  settings = {};
  /** @type {Browser} */
  browser;
  /** @type {Page} */
  page;

  constructor(settings) {
    this.settings = settings;
    this.result = new ServiceResult(Status.blank, 'instance', []);
  }

  #ObterImagemVideo() {
    return `${this.settings.pasta}/${this.settings.nomeDoArquivo}`;
  }

  #ExisteArquivoImagemVideo() {
    if (this.settings.usarArquivosExistentes && utils.existeArquivo(this.#ObterImagemVideo())) return true;

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
      width: ~~this.settings.largura,
      height: ~~this.settings.altura,
    });
  }

  async #NavegarParaImagem() {
    await this.page.goto(this.settings.link, { waitUntil: 'networkidle2' });
    await this.#Esperar(8);
  }

  async #SalvarPrint(salvar) {
    await this.page.screenshot({ path: salvar });
  }

  async Executar() {
    try {
      const arquivoImagemCache = this.#ExisteArquivoImagemVideo();

      if (arquivoImagemCache) return new ServiceResult(Status.ok, 'Arquivo gerado com sucesso', `${this.#ObterImagemVideo()}`);

      await this.#AbrirBrowser();
      await this.#AbrirPagina();

      if (!arquivoImagemCache) {
        await this.#NavegarParaImagem();
        await this.#SalvarPrint(this.#ObterImagemVideo());
      }

      await this.#FecharBrowser();
      return new ServiceResult(Status.ok, 'Arquivo gerado com sucesso', `${this.#ObterImagemVideo()}`);
    } catch (ex) {
      this.#FecharBrowser();
      throw ex;
    }
  }
};
