const fs = require('fs');
const path = require('path');
var child_process = require('child_process');
const pptr = require('puppeteer');
const pastas = require('./../utils/gerenciador-pastas');
const utils = require('./../utils/utils');
const ServicoFala = require('./gerar-fala');
const statusCodes = require('./../utils/services-status-codes');
const processoServiceContructor = require('./processo-service');
const Processo = require('./../model/processo');
const logger = require('./logger');
const { ServicoFalaSettings, ServiceResult } = require('../model/retornoServicos');

module.exports = function (processo, datasourceUrl) {
  const UID = processo;

  const processoService = new processoServiceContructor(datasourceUrl);

  const log = (m) => logger.log(UID, 'gerar-video-processo', m);

  const obterPastaDoProcesso = () => `${pastas.obterPastaArquivosDoDia()}/${UID}`;
  const obterNomeArquivo = (ext) => `${UID}.${ext}`;
  const obterArquivoPrint = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('png')}`;
  const obterArquivoThumb = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('thumb.png')}`;
  const obterArquivoAudio = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('mp3')}`;
  const obterArquivoVideo = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('mp4')}`;
  const obterArquivoBat = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('bat')}`;
  const obterArquivoBatTituloClipboard = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('attr.title.ps1')}`;
  const obterArquivoBatDescricaoClipboard = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('attr.description.ps1')}`;
  const obterArquivoAtributos = () => `${obterPastaDoProcesso()}/${obterNomeArquivo('txt')}`;

  const GerarImagem = async (urlPrint, w, h, urlThumb = null) => {
    const browser = await pptr.launch({ headless: false, ignoreDefaultArgs: ['--disable-extensions'] });
    const page = await browser.newPage();
    await page.setViewport({ width: ~~w, height: ~~h });
    await page.goto(urlPrint, { waitUntil: 'networkidle2' });
    await utils.sleep(8);
    await page.screenshot({ path: obterArquivoPrint() });

    if (urlThumb) {
      await page.goto(urlThumb, { waitUntil: 'networkidle2' });
      await utils.sleep(8);
      await page.screenshot({ path: obterArquivoThumb() });
    }

    await browser.close();
  };

  const GerarFala = async (texto) => {
    const servicoFala = new ServicoFala(UID, new ServicoFalaSettings(obterPastaDoProcesso(), obterNomeArquivo('mp3'), texto, true));
    const resultado = await servicoFala.Executar();
    if (resultado.status !== statusCodes.ok) throw new Error(resultado.mensagem);
  };

  const GerarVideo = async () => {
    fs.writeFileSync(obterArquivoBat(), `ffmpeg -loop 1 -i ${obterArquivoPrint()} -i ${obterArquivoAudio()} -c:v libx264 -c:a copy -shortest ${obterArquivoVideo()}`);
  };

  const GerarArquivoAtributos = (titulo, descricao, roteiro) => {
    fs.writeFileSync(obterArquivoAtributos(), `[TITLE]\n\n${titulo}\n\n[DESCRIPTION]\n\n${descricao}\n\n[ROTEIRO]\n\n${roteiro.replace(/:/g, '\n')}`);

    fs.writeFileSync(obterArquivoBatTituloClipboard(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${titulo}")`, { encoding: 'ascii' });

    fs.writeFileSync(obterArquivoBatDescricaoClipboard(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${descricao}")`, { encoding: 'ascii' });
  };

  const ExecutarBat = () => {
    const bat = path.join(obterArquivoBat());
    log(`start ${bat}`);
    child_process.exec(`call ${bat}`, (error, stdout, stderr) => log(stdout));
  };

  this.Executar = async () => {
    try {
      log(`obtendo dados do processo`);
      const processo = new Processo(await processoService.ObterProcesso());
      utils.criarPastaSeNaoExistir(obterPastaDoProcesso());
      if (processo.tipoLink == 'print') {
        log(`imagem do processo é do tipo print`);
        log(`obtendo imagem do processo`);
        await GerarImagem(processo.link, processo.imgLargura, processo.imgAltura, processo.linkThumb);
      } else {
        log(`imagem do processo é de um tipo não implementado`);
        throw new Erro('processos com link tipo image ainda não foram implementados');
      }
      log(`gerando atributos do video`);
      GerarArquivoAtributos(processo.attrTitulo, processo.attrDescricao, processo.roteiro);
      log(`gerando fala do roteiro`);
      await GerarFala(processo.roteiro);
      log(`gerando bat de video`);
      await GerarVideo();
      log(`gerando video`);
      ExecutarBat();

      log(`atualizando status do processo para sucesso`);
      await processoService.AtualizarProcessoSucesso(obterArquivoVideo());
      return new ServiceResult(statusCodes.ok, 'sucesso');
    } catch (ex) {
      await processoService.AtualizarProcessoErro(JSON.stringify(ex));
      return new ServiceResult(statusCodes.erro, ex.message);
    } finally {
      await processoService.AtualizarLogProcesso();
    }
  };

  return this;
};
