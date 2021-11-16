const fs = require('fs');
const path = require('path');
var child_process = require('child_process');
const pptr = require('puppeteer');
const pastas = require('./../utils/gerenciador-pastas');
const utils = require('./../utils/utils');
const servicoGerarFala = require('./gerar-fala');
const statusCodes = require('./../utils/services-status-codes');
const processoServiceContructor = require('./processo-service');
const Processo = require('./../model/processo');
const moment = require('moment');

module.exports = function (processo, datasourceUrl) {
  const UID = processo;

  const processoService = new processoServiceContructor(datasourceUrl);
  const stack = [];

  this.log = (m) => {
    const now = moment().format("DD/MM/YYYY HH:mm:ss");
    const message = `[${UID}][${now}]\n\t${m}`;
    stack.push(message);
    console.log(message);
  };

  this.obterPastaDoProcesso = () => `${pastas.obterPastaArquivosDoDia()}/${UID}`;
  this.obterNomeArquivo = (ext) => `${UID}.${ext}`;

  this.obterArquivoPrint = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('png')}`;
  this.obterArquivoThumb = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('thumb.png')}`;
  this.obterArquivoAudio = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('mp3')}`;
  this.obterArquivoVideo = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('mp4')}`;
  this.obterArquivoBat = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('bat')}`;
  this.obterArquivoBatTituloClipboard = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('attr.title.ps1')}`;
  this.obterArquivoBatDescricaoClipboard = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('attr.description.ps1')}`;
  this.obterArquivoAtributos = () => `${this.obterPastaDoProcesso()}/${obterNomeArquivo('txt')}`;

  this.ObterProcesso = async () => {
    return await processoService.ObterProcesso();
  };

  this.AtualizarLogProcesso = async () => {
    return await processoService.AtualizarLogProcesso(messageStack.join('\n'));
  };

  this.AtualizarProcessoErro = async (erro) => {
    return await processoService.AtualizarProcessoErro(erro);
  };

  this.AtualizarProcessoSucesso = async (arquivo) => {
    return await processoService.AtualizarProcessoSucesso(arquivo);
  };

  this.GerarImagem = async (urlPrint, w, h, urlThumb = null) => {
    const browser = await pptr.launch({ headless: false, ignoreDefaultArgs: ['--disable-extensions'] });
    const page = await browser.newPage();
    await page.setViewport({ width: ~~w, height: ~~h });
    await page.goto(urlPrint, { waitUntil: 'networkidle2' });
    await utils.sleep(8);
    await page.screenshot({ path: this.obterArquivoPrint() });

    if (urlThumb) {
      await page.goto(urlThumb, { waitUntil: 'networkidle2' });
      await utils.sleep(8);
      await page.screenshot({ path: this.obterArquivoThumb() });
    }

    await browser.close();
  };

  this.GerarFala = async (texto) => {
    var resultado = await servicoGerarFala(UID, {
      salvarEm: this.obterPastaDoProcesso(),
      nomeArquivo: this.obterNomeArquivo('mp3'),
      roteiro: texto,
      usarArquivosExistentes: true,
    }).Executar();
    if (resultado.status !== statusCodes.ok) throw new Error(resultado.mensagem);
  };

  this.GerarVideo = async () => {
    fs.writeFileSync(this.obterArquivoBat(), `ffmpeg -loop 1 -i ${this.obterArquivoPrint()} -i ${this.obterArquivoAudio()} -c:v libx264 -c:a copy -shortest ${this.obterArquivoVideo()}`);
  };

  this.GerarArquivoAtributos = (titulo, descricao, roteiro) => {
    fs.writeFileSync(this.obterArquivoAtributos(), `[TITLE]\n\n${titulo}\n\n[DESCRIPTION]\n\n${descricao}\n\n[ROTEIRO]\n\n${roteiro.replace(/:/g, '\n')}`);

    fs.writeFileSync(this.obterArquivoBatTituloClipboard(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${titulo}")`, { encoding: 'ascii' });

    fs.writeFileSync(this.obterArquivoBatDescricaoClipboard(), `$PSDefaultParameterValues['*:Encoding'] = 'utf8'\nSet-Clipboard("${descricao}")`, { encoding: 'ascii' });
  };

  this.ExecutarBat = () => {
    var bat = path.join(this.obterArquivoBat());
    this.log(`start ${bat}`);
    child_process.exec(`call ${bat}`, (error, stdout, stderr) => this.log(stdout));
  };

  this.Executar = async () => {
    try {
      this.log(`obtendo dados do processo`);
      var processo = new Processo(await this.ObterProcesso());
      utils.criarPastaSeNaoExistir(this.obterPastaDoProcesso());
      if (processo.tipoLink == 'print') {
        this.log(`imagem do processo é do tipo print`);
        this.log(`obtendo imagem do processo`);
        await this.GerarImagem(processo.link, processo.imgLargura, processo.imgAltura, processo.linkThumb);
      } else {
        this.log(`imagem do processo é de um tipo não implementado`);
        throw new Erro('processos com link tipo image ainda não foram implementados');
      }
      this.log(`gerando atributos do video`);
      this.GerarArquivoAtributos(processo.attrTitulo, processo.attrDescricao, processo.roteiro);
      this.log(`gerando fala do roteiro`);
      await this.GerarFala(processo.roteiro);
      this.log(`gerando bat de video`);
      await this.GerarVideo();
      this.log(`gerando video`);
      this.ExecutarBat();

      this.log(`atualizando status do processo para sucesso`);
      await this.AtualizarProcessoSucesso(this.obterArquivoVideo());
    } catch (ex) {
      await this.AtualizarProcessoErro(JSON.stringify(ex));
      throw new Error(ex);
    } finally {
      //await this.AtualizarLogProcesso();
    }
  };

  this.AbrirPasta = () => {
    var pasta = path.resolve(this.obterPastaDoProcesso());
    this.log(`explorer ${pasta}`);
    child_process.exec(`explorer.exe "${pasta}"`, (error, stdout, stderr) => this.log(stdout));
  };

  return this;
};
