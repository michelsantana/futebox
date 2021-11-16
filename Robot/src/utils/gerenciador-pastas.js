const moment = require('moment');
const path = require('path');
const utils = require('./utils');

require('dotenv').config();

const caminhoPastaSaidaDosArquivos = path.resolve(process.env.caminhoPastaSaidaDosArquivos);
const caminhoPastaDownloadPadraoChromium = path.resolve(process.env.caminhoPastaDownloadPadraoChromium);

if(!caminhoPastaSaidaDosArquivos) throw new Error("Por favor configurar arquivo .env com a chave caminhoPastaSaidaDosArquivos apontando para pasta onde deseja gerar os arquivos do bot")
if(!caminhoPastaDownloadPadraoChromium) throw new Error("Por favor configurar arquivo .env com a chave caminhoPastaDownloadPadraoChromium apontando para pasta de download padrão do chrome.")

const obterPastaArquivos = () => `${caminhoPastaSaidaDosArquivos}/`;
const obterPastaArquivosDoDia = () => `${caminhoPastaSaidaDosArquivos}/${moment().format('yyyyMMDD')}/`;
const obterPastaDownloadsChrome = () => `${caminhoPastaDownloadPadraoChromium}/`;
const obterPastaDeRecursos = () => path.resolve(`./resources/`);
const obterPastaRaiz = () => path.resolve(`./`);

utils.criarPastaSeNaoExistir(obterPastaArquivos());
utils.criarPastaSeNaoExistir(obterPastaArquivosDoDia());
utils.criarPastaSeNaoExistir(obterPastaDownloadsChrome());
utils.criarPastaSeNaoExistir(obterPastaDeRecursos());

module.exports = {  
    obterPastaArquivos,
    obterPastaArquivosDoDia,
    obterPastaDownloadsChrome,
    obterPastaDeRecursos,
    obterPastaRaiz
}