const fs = require('fs');
const pptr = require('puppeteer');
const pastas = require('./../utils/gerenciador-pastas');
const statusCodes = require('./../utils/services-status-codes');
const processoServiceContructor = require('./processo-service');
const Processo = require('./../model/processo');
const moment = require('moment');
const logger = require('./logger');
const { ServiceResult } = require('./../model/retornoServicos');

module.exports = function (uid, datasourceUrl) {
  const UID = uid;

  const log = (m) => logger.log(UID, 'upload-youtube', m);
  const processoService = new processoServiceContructor(datasourceUrl);

  const obterPastaRaiz = () => pastas.obterPastaRaiz();
  const obterPastaChrome = () => `${obterPastaRaiz()}/node_modules/puppeteer/.local-chromium/win64-901912/chrome-win/`;
  const obterBat = () => `${obterPastaChrome()}remote.bat`;

  const CriarArquivoBat = () => {
    fs.writeFileSync(obterBat(), `start ${obterPastaChrome()}chrome --remote-debugging-port=%1`);
  };

  this.Executar = async () => {
    try {
      log('obtendo processo');
      const processo = new Processo(await processoService.ObterProcesso());
      const portaExecucao = processo.portaExecucao || '21998';
      log('processo carregado');

      const urlCanal = 'https://studio.youtube.com/channel/UCWs2h6plWKR8xCZM3ljNGRw';

      if (!fs.existsSync(obterBat())) {
        log('criando bat');
        CriarArquivoBat();
      }

      const espera = async (segundos) => {
        return new Promise((res, rej) => setTimeout(res, segundos * 1000));
      };

      const browser = await pptr.launch({
        headless: false,
        args: [`--user-data-dir=${process.env.USERPROFILE}\\AppData\\Local\\Chromium\\User Data`, `--profile-directory=Profile 1`],
      });

      log('conectado');
      await espera(2);

      const page = await browser.newPage();

      log('navegando');
      await page.setViewport({ width: 1280, height: 720 });
      await page.goto(urlCanal, { waitUntil: 'networkidle2' });

      await espera(2);

      const isLoginPage = await page.evaluate(() => document.querySelectorAll('[type="email"]').length > 0);

      if (isLoginPage) {
        log('por favor fazer login');
        return new ServiceResult(statusCodes.authFailed, 'LOGIN');
      }
      log('logado');

      await page.click('#create-icon');
      await espera(1);

      await page.click('#text-item-0');
      await espera(1);

      log('subindo video');
      const elementUploadHandle = await page.$('[type="file"]');
      await elementUploadHandle.uploadFile(processo.arquivoVideo);

      const PreencherCampo = async (selector, text) => {
        await page.click(selector);
        await page.waitForTimeout(700);
        await page.keyboard.down('ControlLeft');
        await page.waitForTimeout(700);
        await page.keyboard.press('A');
        await page.waitForTimeout(700);
        await page.keyboard.up('ControlLeft');
        await page.waitForTimeout(700);

        await page.keyboard.down('Backspace');
        await page.waitForTimeout(700);

        await page.keyboard.type(text);
      };

      const Titulo = async (titulo) => {
        const seletorTitulo = '.input-container.title #textbox';
        await PreencherCampo(seletorTitulo, titulo);
      };

      const Descricao = async (descricao) => {
        const seletorDescricao = '.input-container.description #textbox';
        await PreencherCampo(seletorDescricao, descricao);
      };

      const Playlist = async (nomePlaylist) => {
        const seletorCampoPlaylist = '.dropdown.style-scope.ytcp-video-metadata-playlists';
        const seletorConcluir = '.done-button.action-button.style-scope.ytcp-playlist-dialog';

        await page.click(seletorCampoPlaylist);
        await espera(1);

        const clickId = await page.evaluate((t) => {
          let id = '';
          const seletorLista = '.checkbox-label.style-scope.ytcp-checkbox-group';
          const seletorNomeDoItem = '.label.label-text.style-scope.ytcp-checkbox-group';
          const itens = document.querySelectorAll(seletorLista);

          Array.from(itens).forEach((_) => {
            const item = _.querySelector(seletorNomeDoItem)?.innerHTML;
            if (item && item.toLowerCase()?.indexOf(t.toLowerCase()) > -1) id = _.id;
          });
          return id;
        }, nomePlaylist);

        await page.click(`#${clickId}`);
        await espera(1);
        await page.click(seletorConcluir);
      };

      const Proximo = async () => {
        const seletorRodapeJanela = '.button-area.ytcp-uploads-dialog';
        await page.click(`${seletorRodapeJanela} #next-button`);
      };

      const Publicar = async () => {
        const seletorRodapeJanela = '.button-area.ytcp-uploads-dialog';
        await page.click(`${seletorRodapeJanela} #done-button`);
      };

      const Visibilidade = async (visibilidade) => {
        if (!visibilidade || visibilidade == 'public') return;

        if (visibilidade == 'privado') await page.click('[name="PRIVATE"]');
        if (visibilidade == 'naolistado') await page.click('[name="UNLISTED"]');
      };

      const nomePlaylist = {
        partida: 'shorts',
        classificacao: 'classi',
        rodada: 'calen',
      };

      await espera(10);

      log('preenchendo campos');
      await Titulo(processo.attrTitulo);
      await espera(1);
      await Descricao(processo.attrDescricao);
      await espera(1);
      await Playlist(nomePlaylist[processo.tipo]);
      await espera(1);

      await Proximo();
      await espera(1);
      await Proximo();
      await espera(1);
      await Proximo();
      await espera(1);

      await Visibilidade('public');
      await espera(2);
      await Publicar();

      await espera(4);
      log('desconectando');
      browser.close();

      log('fim');
    } catch (ex) {
      log(`Erro: ${JSON.stringify(ex)}`);
    } finally {
      await processoService.AtualizarLogProcesso(messageStack.join('\n'));
    }
  };

  this;
};


// const ConectarBrowser = async () => {
//   log(`conectando browser na porta ${portaExecucao}`);
//   const browserURL = `http://127.0.0.1:${portaExecucao}`;

//   child_process.exec(`${obterBat()} ${portaExecucao}`, (error, stdout, stderr) => {});

//   await espera(2);
//   return await pptr.connect({ browserURL: browserURL });
// };