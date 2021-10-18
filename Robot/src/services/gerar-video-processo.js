const axios = require("axios").default;
const fs = require("fs");
const path = require("path");
var child_process = require("child_process");
const pptr = require("puppeteer");
const pastas = require("./../utils/gerenciador-pastas");
const utils = require("./../utils/utils");
const servicoGerarFala = require("./gerar-fala");
const statusCodes = require("./../utils/services-status-codes");

const enumTipo = {
  [0]: "partida",
  [1]: "classificacao",
  [2]: "rodada",
};

const enumTipoLink = {
  [0]: "print",
  [1]: "image",
};

function Processo(processo) {
  this.tipo = enumTipo[processo.tipo];
  this.idExterno = processo.idExterno;
  this.nome = processo.nome;
  this.link = processo.link;
  this.tipoLink = enumTipoLink[processo.tipoLink];
  this.imgLargura = processo.imgLargura;
  this.imgAltura = processo.imgAltura;
  this.roteiro = processo.roteiro;
  this.attrTitulo = processo.attrTitulo;
  this.attrDescricao = processo.attrDescricao;
  this.status = processo.status;
  this.processado = processo.processado;
  this.json = processo.json;
  this.linkThumb = processo.linkThumb;
  return this;
}

module.exports = function (processo, datasourceUrl) {
  const UID = processo;

  this.obterPastaDoProcesso = () =>
    `${pastas.obterPastaArquivosDoDia()}/${UID}`;
  this.obterNomeArquivo = (ext) => `${UID}.${ext}`;

  this.obterArquivoPrint = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("png")}`;
  this.obterArquivoThumb = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("thumb.png")}`;
  this.obterArquivoAudio = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("mp3")}`;
  this.obterArquivoVideo = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("mp4")}`;
  this.obterArquivoBat = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("bat")}`;
  this.obterArquivoBatTituloClipboard = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("title.bat")}`;
  this.obterArquivoBatDescricaoClipboard = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("description.bat")}`;
  this.obterArquivoAtributos = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("txt")}`;

  this.ObterProcesso = async () => {
    return (await axios.get(`${datasourceUrl}/obter`)).data;
  };

  this.AtualizarProcessoErro = async (erro) => {
    return (await axios.get(`${datasourceUrl}/erro?mensagem=${erro}`)).data;
  };

  this.AtualizarProcessoSucesso = async () => {
    return (await axios.get(`${datasourceUrl}/sucesso`)).data;
  };

  this.GerarImagem = async (urlPrint, w, h, urlThumb = null) => {
    const browser = await pptr.launch({
      headless: false,
      ignoreDefaultArgs: ["--disable-extensions"],
    });
    const page = await browser.newPage();
    await page.setViewport({ width: ~~w, height: ~~h });
    await page.goto(urlPrint, { waitUntil: "networkidle2" });
    await utils.sleep(8);
    await page.screenshot({ path: this.obterArquivoPrint() });

    if (urlThumb) {
      await page.goto(urlThumb, { waitUntil: "networkidle2" });
      await utils.sleep(8);
      await page.screenshot({ path: this.obterArquivoThumb() });
    }

    await browser.close();
  };

  this.GerarFala = async (texto) => {
    var resultado = await servicoGerarFala(UID, {
      salvarEm: this.obterPastaDoProcesso(),
      nomeArquivo: this.obterNomeArquivo("mp3"),
      roteiro: texto,
      usarArquivosExistentes: true,
    }).Executar();
    if (resultado.status !== statusCodes.ok)
      throw new Error(resultado.mensagem);
  };

  this.GerarVideo = async () => {
    fs.writeFileSync(
      this.obterArquivoBat(),
      `ffmpeg -loop 1 -i ${this.obterArquivoPrint()} -i ${this.obterArquivoAudio()} -c:v libx264 -c:a copy -shortest ${this.obterArquivoVideo()}`
    );
  };

  this.GerarArquivoAtributos = (titulo, descricao) => {
    fs.writeFileSync(
      this.obterArquivoAtributos(),
      `[TITLE]\n${titulo}\n[DESCRIPTION]\n${descricao}`
    );
    // fs.writeFileSync(
    //   this.obterArquivoBatTituloClipboard(),
    //   `echo ${titulo}|clip`
    // );
    // fs.writeFileSync(
    //   this.obterArquivoBatDescricaoClipboard(),
    //   `echo ${descricao}|clip`
    // );
  };

  this.ExecutarBat = () => {
    var bat = path.join(this.obterArquivoBat());
    this.log(`start ${bat}`);
    child_process.exec(`call ${bat}`, (error, stdout, stderr) =>
      this.log(stdout)
    );
  };

  this.log = (msg) => {
    console.log(`${UID} - ${msg}`);
  };

  this.Executar = async () => {
    try {
      this.log(`Obtendo dados do processo`);
      var processo = new Processo(await this.ObterProcesso());
      utils.criarPastaSeNaoExistir(this.obterPastaDoProcesso());
      if (processo.tipoLink == "print") {
        this.log(`Imagem do processo é do tipo print`);
        this.log(`Obtendo imagem do processo`);
        await this.GerarImagem(
          processo.link,
          processo.imgLargura,
          processo.imgAltura,
          processo.linkThumb
        );
      } else {
        this.log(`Imagem do processo é de um tipo não implementado`);
        throw new Erro(
          "processos com link tipo image ainda não foram implementados"
        );
      }
      this.log(`Gerando atributos do video`);
      this.GerarArquivoAtributos(processo.attrTitulo, processo.attrDescricao);
      this.log(`Gerando fala do roteiro`);
      await this.GerarFala(processo.roteiro);
      this.log(`Gerando bat de video`);
      await this.GerarVideo();
      this.log(`Atualizando status do processo para sucesso`);
      await this.AtualizarProcessoSucesso();
      this.log(`Gerando video`);
      this.ExecutarBat();
    } catch (ex) {
      await this.AtualizarProcessoErro(JSON.stringify(ex));
      throw new Error(ex);
    }
  };

  this.AbrirPasta = () => {
    var pasta = path.resolve(this.obterPastaDoProcesso());
    this.log(`explorer ${pasta}`);
    child_process.exec(`explorer.exe "${pasta}"`, (error, stdout, stderr) =>
      this.log(stdout)
    );
  };

  return this;
};
