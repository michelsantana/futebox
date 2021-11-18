const pptr = require('puppeteer');
const { Browser, Page } = require('puppeteer');

const Settings = require('../model/settings');
const ServiceResult = require('../model/serviceresult');
const Status = require('../model/statuscodes');
const pastas = require('./../utils/gerenciador-pastas');
const utils = require('./../utils/utils');

module.exports = class AudioService {
  /** @type {Settings} */
  settings;
  /** @type {Browser} */
  browser;
  /** @type {Page} */
  page;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  #ObterArquivoNaPastaDownloads() {
    return `${pastas.obterPastaDownloadsChrome()}/${this.settings.nomeArquivoDestino}.mp3`;
  }

  #ObterArquivoNoDestino() {
    return `${this.settings.pastaDestino}/${this.settings.nomeArquivoDestino}.mp3`;
  }

  #ExisteArquivoBaixado() {
    if (this.settings.usarArquivosExistentes && utils.existeArquivo(this.#ObterArquivoNaPastaDownloads())) {
      this.#MoverArquivoBaixadoParaDestino();
      return true;
    }
  }

  #ExisteArquivoBaixadoMovido() {
    if (this.settings.usarArquivosExistentes && utils.existeArquivo(this.#ObterArquivoNoDestino())) {
      return true;
    }
  }

  async #MoverArquivoBaixadoParaDestino() {
    utils.moverArquivo(this.#ObterArquivoNaPastaDownloads(), this.#ObterArquivoNoDestino());
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
    await this.page.setViewport({ width: 1280, height: 720 });
    await this.page.goto('https://www.ibm.com/demos/live/tts-demo/self-service/home', { waitUntil: 'networkidle2' });
  }

  async #EsperarElemento(seletor, timeout = null) {
    if (timeout) await this.page.waitForSelector(seletor, { timeout: timeout });
    else await this.page.waitForSelector(seletor);
  }

  async #Esperar(timeoutMs) {
    await utils.sleep(timeoutMs);
  }

  async #ConfigurarParametrosPagina() {
    await this.page.click('#text-area');
    await this.#Esperar(0.7);
    await this.page.keyboard.down('ControlLeft');
    await this.#Esperar(0.7);
    await this.page.keyboard.press('A');
    await this.#Esperar(0.7);
    await this.page.keyboard.up('ControlLeft');
    await this.#Esperar(0.7);

    await this.page.keyboard.down('Backspace');
    await this.#Esperar(0.7);

    await this.page.keyboard.type(this.settings.processo.roteiro);

    await this.#Esperar(0.7);

    await this.page.click('#slider');
    await this.#Esperar(0.7);

    await this.page.keyboard.press('ArrowRight');
    await this.#Esperar(0.7);

    await this.page.click('#downshift-3-toggle-button');
    await this.#Esperar(0.7);
  }

  async #InjetarScriptBaixarAudio() {
    await this.page.evaluate((nomeDoArquivoDownload) => {
      let endlink = document.createElement('button');
      endlink.innerHTML = 'FinalizarForçado';
      endlink.setAttribute('class', 'dwlend');
      endlink.setAttribute('onclick', 'this.id="dwlend"');
      endlink.setAttribute('style', `position:absolute;top:150px;left:0;z-index:99999999999999;font-size:50px;background:#F00;width:100%;`);
      document.body.append(endlink);

      var audio = document.querySelector('audio');
      audio.onplay = function () {
        let link = document.createElement('a');
        link.innerHTML = 'IndicadorDeInicialização';
        link.setAttribute(`href`, audio.src);
        link.setAttribute(`download`, nomeDoArquivoDownload);
        link.setAttribute('id', 'dwl');
        link.setAttribute('style', `position:absolute;top:50px;left:0;z-index:99999999999999;font-size:50px;background:#F00;width:100%;`);
        document.body.append(link);
      };

      audio.onended = function () {
        let link = document.createElement('a');
        link.innerHTML = 'IndicadorDeFinalização';
        link.setAttribute(`href`, '#');
        link.setAttribute('id', 'dwlend');
        link.setAttribute('style', `position:absolute;top:100px;left:0;z-index:99999999999999;font-size:50px;background:#F00;width:100%;`);
        document.body.append(link);
      };
    }, this.settings.nomeArquivoDestino);
  }

  async #TocarAudio() {
    await this.page.click('.play-btn.bx--btn');
    await this.#EsperarElemento('audio[src]');
  }

  async #IniciarDownloadAudio() {
    await this.#EsperarElemento('#dwl', 60000);
    await this.page.click('#dwl');
  }

  async #AguardarDownload() {
    var tentativas = 1;
    while (tentativas < 12 * 3) {
      if (utils.existeArquivo(this.#ObterArquivoNaPastaDownloads())) {
        await this.page.evaluate(() => (document.querySelector('.dwlend').id = 'dwlend'));
        break;
      } else await this.#Esperar(5);
      tentativas++;
    }
    await this.page.waitForSelector('#dwlend', { timeout: 60000 * 5 }); // espera até 5 minutos
  }

  async Executar() {
    try {
      if (this.#ExisteArquivoBaixado()) {
        return new ServiceResult(Status.ok, 'Arquivo do cache', this.#ObterArquivoNoDestino());
      }

      if (this.#ExisteArquivoBaixadoMovido()) {
        return new ServiceResult(Status.ok, 'Arquivo do cache', this.#ObterArquivoNoDestino());
      }

      await this.#AbrirBrowser();
      await this.#AbrirPagina();
      await this.#EsperarElemento('audio');
      await this.#ConfigurarParametrosPagina();
      await this.#InjetarScriptBaixarAudio();
      await this.#TocarAudio();
      await this.#IniciarDownloadAudio();
      await this.#AguardarDownload();
      await this.#MoverArquivoBaixadoParaDestino();

      await this.#FecharBrowser();
      return new ServiceResult(Status.ok, 'Arquivo gerado com sucesso', this.#ObterArquivoNoDestino());
    } catch (ex) {
      this.#FecharBrowser();
      throw ex;
    }
  }
};
