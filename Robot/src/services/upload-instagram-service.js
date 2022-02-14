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

  constructor(settings) {
    this.settings = settings;
  }

  async #Esperar(timeoutMs) {
    await utils.sleep(timeoutMs);
  }

  async #AbrirBrowser() {
    const profileDir = process.env.USERPROFILE;
    this.browser = await pptr.launch({
      executablePath: `${profileDir}\\AppData\\Local\\Google\\Chrome SxS\\Application\\chrome.exe`,
      headless: false,
      userDataDir: `${profileDir}\\AppData\\Local\\Google\\Chrome SxS\\User Data`,
      defaultViewport: { width: 1920, height: 1080 },
      args: [
        `--user-data-dir=${profileDir}\\AppData\\Local\\Google\\Chrome SxS\\User Data\\`,
        `--profile-directory="Profile 1"`
      ],
    });
  }

  async #FecharBrowser() {
    await this.browser?.close();
  }

  async #AbrirPagina() {
    this.page = await this.browser.newPage();
    await this.page.setViewport({ width: 1280, height: 720 });
    await this.page.goto('https://www.instagram.com/', { waitUntil: 'networkidle2' });
    await this.#Esperar(2);
  }

  async #EstaLogado() {
    const existeBotaoDePublicacao = await this.page.evaluate(() => document.querySelectorAll('[aria-label="Nova publicação"]').length > 0);
    if (existeBotaoDePublicacao) {
      return true;
    }
    return false;
  }

  async #AbrirTelaDeUpload() {
    await this.page.click('[aria-label="Nova publicação"]');
    await this.#Esperar(1);
  }

  async #RealizarUploadDoVideo() {
    const elementUploadHandle = await this.page.$('[aria-label="Criar nova publicação"] [type="file"]');
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

  async #PreencherCampoLegenda() {
    await this.#PreencherCampo('textarea[aria-label="Escreva uma legenda..."]', `${this.settings.legendaPostagem}`);
    await this.#Esperar(1);
  }

  async #SelecionarLegendasAutomaticas() {
    await this.page.evaluate(() => {
      document.querySelectorAll('[role="button"]').forEach((_) => {
        if (_.innerHTML.toLowerCase().indexOf('acessi') > -1) _.click();
      });
    });
    await this.#Esperar(1);
    await this.page.evaluate(() => {
      document.querySelectorAll('label > input').forEach((_) => {
        if (!_.checked && _.parentElement.parentElement.innerHTML.toLowerCase().indexOf('legendas geradas') > -1) _.click();
      });
    });
    await this.#Esperar(1);
  }

  async #ClicarEmAvancar() {
    await this.page.evaluate(() => {
      document.querySelectorAll('[role="dialog"] button').forEach((_) => {
        if (_.innerHTML.toLowerCase().indexOf('avan') > -1) _.click();
      });
    });
    await this.#Esperar(1);
  }

  async #ClicarEmPublicar() {
    if(this.settings.debug) return;

    await this.page.evaluate(() => {
      document.querySelectorAll('[role="dialog"] button').forEach((_) => {
        if (_.innerHTML.toLowerCase().indexOf('compart') > -1) _.click();
      });
    });
    await this.#Esperar(4);
  }

  async #SelecionarDimensao() {
    await this.page.evaluate(() => {
      document.querySelector('[aria-label="Selecionar corte"]').parentElement.parentElement.click();
    });
    await this.#Esperar(1);
    await this.page.evaluate(() => {
      Array.from(document.querySelectorAll('[aria-label="Cortar"] button'))
        .filter((_) => _.innerHTML.indexOf('>Original<') > -1)
        .forEach((_) => {
          _.click();
        });
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
      await this.#SelecionarDimensao();
			
			await this.#ClicarEmAvancar();
			await this.#ClicarEmAvancar();

      await this.#PreencherCampoLegenda();
      await this.#SelecionarLegendasAutomaticas();
      await this.#ClicarEmPublicar();
      
      await this.#FecharBrowser();
      return new ServiceResult(Status.ok, 'Video publicado');
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
