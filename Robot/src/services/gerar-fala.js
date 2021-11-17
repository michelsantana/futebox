const pptr = require('puppeteer');
const utils = require('./../utils/utils');
const statusCodes = require('./../utils/services-status-codes');
const pastas = require('./../utils/gerenciador-pastas');
const logger = require('./logger');
const { ServicoFalaRetorno, ServicoFalaSettings } = require('./../model/retornoServicos');

module.exports = function (uid, settings = new ServicoFalaSettings()) {
  const log = (m) => logger.log(uid, 'gerar-fala', m);
  const defaults = new ServicoFalaSettings(pastas.obterPastaArquivos(), `${uid}.mp3`, `${uid} - sem roteiro`, false);

  settings = Object.assign(settings, defaults);

  const obterArquivoPastaDownloads = () => `${pastas.obterPastaDownloadsChrome()}/${settings.nomeArquivo}`;
  const obterArquivoDestino = () => `${settings.pasta}/${settings.arquivo}`;

  utils.criarPastaSeNaoExistir(settings.pasta);

  const ExisteArquivoBaixado = () => utils.existeArquivo(obterArquivoPastaDownloads());
  const ExisteArquivoBaixadoMovido = () => utils.existeArquivo(obterArquivoDestino());
  const MoverArquivoBaixadoParaDestino = () => utils.moverArquivo(obterArquivoPastaDownloads(), obterArquivoDestino());

  let intervaloFinalizacaoDownload = null;
  const EsperarFinalizacaoDownload = async (page) => {
    try {
      function exec() {
        return setTimeout(() => {
          if (ExisteArquivoBaixado()) page.evaluate(() => (document.querySelector('.dwlend').id = 'dwlend'));
          else intervaloFinalizacaoDownload = exec();
        }, 3000);
      }
      intervaloFinalizacaoDownload = exec();
      await page.waitForSelector('#dwlend', { timeout: 60000 * 5 }); // espera até 5 minutos
      clearTimeout(intervaloFinalizacaoDownload);
    } catch (ex) {
      clearTimeout(intervaloFinalizacaoDownload);
      return new ServicoFalaRetorno(statusCodes.erro, ex);
    }
  };

  this.Executar = async () => {
    if (ExisteArquivoBaixado() && usarArquivosExistentes) {
			log('usando cache');
			log('arquivo já foi baixado');
      MoverArquivoBaixadoParaDestino();
			log('movido para destino');
      return new ServicoFalaRetorno(statusCodes.ok, 'Arquivo do cache');
    }
    if (ExisteArquivoBaixadoMovido() && usarArquivosExistentes) {
			log('usando cache');
			log('arquivo já movido');
      return new ServicoFalaRetorno(statusCodes.ok, 'Arquivo do cache');
    }

		log('abrindo browser');
    const browser = await pptr.launch({
      headless: false,
      args: ['--use-fake-ui-for-media-stream'],
      ignoreDefaultArgs: ['--mute-audio'],
    });

    const page = await browser.newPage();

    await page.setViewport({ width: 1280, height: 720 });
    await page.goto('https://www.ibm.com/demos/live/tts-demo/self-service/home', { waitUntil: 'networkidle2' });

		log('navegando para ibm');
    await page.waitForSelector('audio');

		log('configurando parametros');
    await page.evaluate(() => (document.querySelector('#text-area').value = ''));

    await page.type('#text-area', roteiro);
    await page.waitForTimeout(720);

    await page.click('#slider');
    await page.waitForTimeout(720);

    // await page.keyboard.press('ArrowLeft');
    // await page.waitForTimeout(720);
    await page.keyboard.press('ArrowRight');
    await page.waitForTimeout(720);
    // await page.keyboard.press('ArrowRight');
    // await page.waitForTimeout(720);

    await page.click('#downshift-3-toggle-button');
    await page.waitForTimeout(720);

    await page.evaluate((nomeDoArquivoDownload) => {
      let endlink = document.createElement('button');
      endlink.innerHTML = 'FinalizarForçado';
      endlink.setAttribute('class', 'dwlend');
      endlink.setAttribute('onclick', 'this.id="dwlend"');
      endlink.setAttribute('style', `position:absolute;top:150px;left:0;z-index:99999999999999;font-size:50px;background:#000;width:100%;`);
      document.body.append(endlink);

      var audio = document.querySelector('audio');
      audio.onplay = function () {
        let link = document.createElement('a');
        link.innerHTML = 'IndicadorDeInicialização';
        link.setAttribute(`href`, audio.src);
        link.setAttribute(`download`, nomeDoArquivoDownload);
        link.setAttribute('id', 'dwl');
        link.setAttribute('style', `position:absolute;top:50px;left:0;z-index:99999999999999;font-size:50px;background:#000;width:100%;`);
        document.body.append(link);
      };

      audio.onended = function () {
        let link = document.createElement('a');
        link.innerHTML = 'IndicadorDeFinalização';
        link.setAttribute(`href`, '#');
        link.setAttribute('id', 'dwlend');
        link.setAttribute('style', `position:absolute;top:100px;left:0;z-index:99999999999999;font-size:50px;background:#000;width:100%;`);
        document.body.append(link);
      };
    }, nomeArquivo);

		log('tocando');
    await page.click('.play-btn.bx--btn');
    await page.waitForSelector('audio[src]');

    await page.waitForSelector('#dwl', { timeout: 60000 });
    await page.click('#dwl');

    await EsperarFinalizacaoDownload(page);
		log('arquivo baixado');

    MoverArquivoBaixadoParaDestino();
		log('arquivo movido para destino');

    await page.close();
    await browser.close();
		log('concluido');

    return new ServicoFalaRetorno(statusCodes.ok, 'Novo arquivo gerado');
  };

  return this;
};

/*

function descricaoColocacao(n) {
            switch (~~n) {
                case 1 : return 'primeiro';
                case 2 : return 'segundo';
                case 3 : return 'terceiro';
                case 4 : return 'quarto';
                case 5 : return 'quinto';
                case 6 : return 'sexto';
                case 7 : return 'primeiro';
                case 8 : return 'primeiro';
                case 9 : return 'primeiro';
                case 10 : return 'primeiro';
                case 11 : return 'primeiro';
                case 12 : return 'primeiro';
                case 13 : return 'primeiro';
                case 14 : return 'primeiro';
                case 15 : return 'primeiro';
                case 16 : return 'primeiro';
                case 17 : return 'primeiro';
                case 18 : return 'primeiro';
                case 19 : return 'primeiro';
                case 20 : return 'primeiro';
            }
        }
*/
