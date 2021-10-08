const { default: axios } = require("axios");
const fs = require("fs");
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
  return this;
}

module.exports = function (processo, datasourceUrl) {
  const UID = processo;

  this.obterPastaDoProcesso = () =>
    `${pastas.obterPastaArquivosDoDia()}/${UID}`;
  this.obterNomeArquivo = (ext) => `${UID}.${ext}`;

  this.obterArquivoPrint = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("png")}`;
  this.obterArquivoAudio = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("mp3")}`;
  this.obterArquivoBat = () =>
    `${this.obterPastaDoProcesso()}/${obterNomeArquivo("bat")}`;
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

  this.GerarImagem = async (urlPrint, w, h) => {
    const browser = await pptr.launch({ headless: false });
    const page = await browser.newPage();
    await page.setViewport({ width: ~~w, height: ~~h });
    await page.goto(urlPrint, { waitUntil: "networkidle2" });
    await utils.sleep(2);
    await page.screenshot({ path: this.obterArquivoPrint() });
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
      `ffmpeg -loop 1 -i ${this.obterNomeArquivo(
        "png"
      )} -i ${this.obterNomeArquivo(
        "mp3"
      )} -c:v libx264 -c:a copy -shortest ${this.obterNomeArquivo("mp4")}`
    );
  };

  this.GerarArquivoAtributos = (titulo, descricao) => {
    fs.writeFileSync(
      this.obterArquivoAtributos(),
      `[TITLE]\n${titulo}\n[DESCRIPTION]\n${descricao}`
    );
  };

  this.Executar = async () => {
    try {
      var processo = new Processo(await this.ObterProcesso());
      console.log(processo);
      utils.criarPastaSeNaoExistir(this.obterPastaDoProcesso());

      if (processo.tipoLink == "print")
        await this.GerarImagem(
          processo.link,
          processo.imgLargura,
          processo.imgAltura
        );
      else
        throw new Erro(
          "processos com link tipo image ainda não foram implementados"
        );
      this.GerarArquivoAtributos(processo.attrTitulo, processo.attrDescricao);
      await this.GerarFala(processo.roteiro);
      await this.GerarVideo();
      await this.AtualizarProcessoSucesso();
    } catch (ex) {
      await this.AtualizarProcessoErro("Deu erro!");
      throw new Error(ex);
    }
  };
  return this;
};
