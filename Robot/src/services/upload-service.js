const pptr = require('puppeteer');
const { Browser, Page } = require('puppeteer');

const ServiceResult = require('./../model/serviceresult');
const Settings = require('../model/settings');
const Status = require('./../model/statuscodes');
const utils = require('./../utils/utils');

module.exports = class UploadService {
  /** @type {Settings} */
  settings;
  /** @type {Browser} */
  browser;
  /** @type {Page} */
  page;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  async #Esperar(timeoutMs) {
    await utils.sleep(timeoutMs);
  }

  async #AbrirBrowser() {
    this.browser = await pptr.launch({
      headless: false,
      args: [`--user-data-dir=${process.env.USERPROFILE}\\AppData\\Local\\Chromium\\User Data`, `--profile-directory=Profile 1`],
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
    await elementUploadHandle.uploadFile(this.settings.processo.arquivoVideo);
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
    await this.#PreencherCampo('.input-container.title #textbox', this.settings.processo.attrTitulo);
    await this.#Esperar(1);
  }

  async #PreencherCampoDescricao() {
    await this.#PreencherCampo('.input-container.description #textbox', this.settings.processo.attrDescricao);
    await this.#Esperar(1);
  }

  async #SelecionarPlaylist() {
    const nomePlaylist = {
      partida: 'shorts',
      classificacao: 'classi',
      rodada: 'calen',
    };

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
    }, nomePlaylist[this.settings.processo.tipo.desc]);

    await this.page.click(`#${clickId}`);
    await this.#Esperar(1);
    await this.page.click(seletorConcluir);
    await this.#Esperar(1);
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
    const seletorRodapeJanela = '.button-area.ytcp-uploads-dialog';
    await this.page.click(`${seletorRodapeJanela} #done-button`);
    await this.#Esperar(4);
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
      await this.#ClicarEmProximo();
      await this.#ClicarEmProximo();
      await this.#ClicarEmProximo();

      await this.#SelecionarVisibilidade();
      await this.#ClicarEmPublicar();

      await this.#FecharBrowser();
      return new ServiceResult(Status.ok, 'Video publicado');
    } catch (ex) {
      this.#FecharBrowser();
      throw ex;
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
      throw ex;
    }
  }
};
