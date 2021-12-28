const pptr = require('puppeteer');
const { Browser, Page } = require('puppeteer');

const ServiceResult = require('./../model/service-result');
const UploadServiceSettings = require('../model/upload-service-settings');
const Status = require('./../model/statuscodes');
const utils = require('./../utils/utils');
require('dotenv').config();
//const chromiumProfile = process.env.chromiumProfile;

module.exports = class UploadService {
  /** @type {UploadServiceSettings} */
  settings;
  /** @type {Browser} */
  browser;
  /** @type {Page} */
  page;
  headless = false;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  async #Esperar(timeoutMs) {
    await utils.sleep(timeoutMs);
  }

  async #AbrirBrowser() {
    // this.browser = await pptr.launch({
    //   headless: false,
    //   userDataDir: `${process.env.USERPROFILE}\\AppData\\Local\\Chromium\\User Data`,
    //   defaultViewport: { width: 1920, height: 1080 },
    //   args: ['--no-sandbox', '--window-position=0,1080', ],
    // });
    this.browser = await pptr.launch({
      executablePath: `${process.env.USERPROFILE}\\AppData\\Local\\Google\\Chrome SxS\\Application\\chrome.exe`,
      headless: false,
      userDataDir: `${process.env.USERPROFILE}\\AppData\\Local\\Google\\Chrome SxS\\User Data`,
      defaultViewport: { width: 1920, height: 1080 },
      args: [`--user-data-dir=${process.env.USERPROFILE}\\AppData\\Local\\Google\\Chrome SxS\\User Data\\Profile 1`],
    });
  }

  async #FecharBrowser() {
    await this.browser?.close();
  }

  async #AbrirPagina() {
    this.page = await this.browser.newPage();
    await this.page.setViewport({ width: 1280, height: 720 });
    await this.page.goto('https://studio.youtube.com/channel/UCWs2h6plWKR8xCZM3ljNGRw', { waitUntil: 'networkidle2' });
    await this.#Esperar(2);
  }

  async #EstaLogado() {
    const paginaDeLogin = await this.page.evaluate(() => document.querySelectorAll('[type="email"]').length > 0);
    if (paginaDeLogin) {
      return false;
    }
    return true;
  }

  async #AbrirTelaDeUpload() {
    await this.page.click('#create-icon');
    await this.#Esperar(1);

    await this.page.click('#text-item-0');
    await this.#Esperar(1);
  }

  async #RealizarUploadDoVideo() {
    const elementUploadHandle = await this.page.$('[type="file"]');
    await elementUploadHandle.uploadFile(`${this.settings.pasta}/${this.settings.nomeDoArquivoVideo}`);
    await this.#Esperar(10);
  }

  async #PreencherCampo(seletor, texto) {
    await this.page.click(seletor);
    await this.#Esperar(0.7);
    await this.page.keyboard.down('ControlLeft');
    await this.#Esperar(0.7);
    await this.page.keyboard.press('A');
    await this.#Esperar(0.7);
    await this.page.keyboard.up('ControlLeft');
    await this.#Esperar(0.7);

    await this.page.keyboard.down('Backspace');
    await this.#Esperar(0.7);

    await this.page.keyboard.type(texto);
  }

  async #PreencherCampoTitulo() {
    await this.#PreencherCampo('.input-container.title #textbox', this.settings.titulo);
    await this.#Esperar(1);
  }

  async #PreencherCampoDescricao() {
    await this.#PreencherCampo('.input-container.description #textbox', this.settings.descricao);
    await this.#Esperar(1);
  }

  async #SelecionarPlaylist() {
    try {
      const nomePlaylist = { short: 'shorts', classificacao: 'classi', rodada: 'calen' };

      const seletorCampoPlaylist = '.dropdown.style-scope.ytcp-video-metadata-playlists';
      const seletorConcluir = '.done-button.action-button.style-scope.ytcp-playlist-dialog';

      await this.page.click(seletorCampoPlaylist);
      await this.#Esperar(1);

      const clickId = await this.page.evaluate((t) => {
        let id = '';
        const seletorLista = '.checkbox-label.style-scope.ytcp-checkbox-group';
        const seletorNomeDoItem = '.label.label-text.style-scope.ytcp-checkbox-group';
        const itens = document.querySelectorAll(seletorLista);

        Array.from(itens).forEach((_) => {
          const item = _.querySelector(seletorNomeDoItem)?.innerHTML;
          if (item && item.toLowerCase()?.indexOf(t.toLowerCase()) > -1) id = _.id;
        });
        return id;
      }, nomePlaylist[this.settings.playlist]);

      await this.page.click(`#${clickId}`);
      await this.#Esperar(1);
      await this.page.click(seletorConcluir);
      await this.#Esperar(1);
    } catch (ex) {
      console.log('Não foi possível selecionar a playlist');
      console.log(ex);
    }
  }

  async #SelecionarVisibilidade(visibilidade) {
    if (!visibilidade || visibilidade == 'public') return;
    if (visibilidade == 'privado') await this.page.click('[name="PRIVATE"]');
    if (visibilidade == 'naolistado') await this.page.click('[name="UNLISTED"]');
    await this.#Esperar(1);
  }

  async #ClicarEmProximo() {
    const seletorRodapeJanela = '.button-area.ytcp-uploads-dialog';
    await this.page.click(`${seletorRodapeJanela} #next-button`);
    await this.#Esperar(1);
  }

  async #ClicarEmPublicar() {
    if (this.settings.debug) return;
    const seletorRodapeJanela = '.button-area.ytcp-uploads-dialog';
    await this.page.click(`${seletorRodapeJanela} #done-button`);
    await this.#Esperar(4);
  }

  async #ObterLinkDoVideo() {
    return await this.page.$eval('#details .video-url-fadeable a[href]', (element) => {
      return element.innerHTML;
    });
  }

  async Executar() {
    try {
      await this.#AbrirBrowser();
      await this.#AbrirPagina();

      const estaLogado = await this.#EstaLogado();
      if (!estaLogado) {
        await this.#FecharBrowser();
        return new ServiceResult(Status.authFailed, 'Precisa autenticar esse perfil');
      }

      await this.#AbrirTelaDeUpload();
      await this.#RealizarUploadDoVideo();

      await this.#PreencherCampoTitulo();
      await this.#PreencherCampoDescricao();
      await this.#SelecionarPlaylist();

      const linkVideo = await this.#ObterLinkDoVideo();

      await this.#ClicarEmProximo();
      await this.#ClicarEmProximo();
      await this.#ClicarEmProximo();

      await this.#SelecionarVisibilidade();
      await this.#ClicarEmPublicar();

      await this.#FecharBrowser();
      return new ServiceResult(Status.ok, 'Video publicado', linkVideo);
    } catch (ex) {
      this.#FecharBrowser();
      return new ServiceResult(Status.erro, 'Erro inesperado');
    }
  }

  async StatusLogin() {
    try {
      await this.#AbrirBrowser();
      await this.#AbrirPagina();

      const estaLogado = await this.#EstaLogado();
      if (!estaLogado) {
        await this.#FecharBrowser();
        return new ServiceResult(Status.authFailed, 'Precisa autenticar esse perfil');
      }
      await this.#FecharBrowser();

      return new ServiceResult(Status.ok, 'Autenticado');
    } catch (ex) {
      this.#FecharBrowser();
      return new ServiceResult(Status.erro, 'Erro inesperado');
    }
  }
};
