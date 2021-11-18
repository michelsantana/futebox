const fs = require('fs');

module.exports = {
  escreverArquivo: (arquivo, conteudo) => fs.writeFileSync(arquivo, conteudo),
  criarPastaSeNaoExistir: (pasta) => (!fs.existsSync(pasta) ? fs.mkdirSync(pasta, { recursive: true }) : false),
  sleep: async (seconds) => new Promise((resolve) => setTimeout(resolve, seconds * 1000)),
  existeArquivo: (arquivo) => fs.existsSync(arquivo),
  moverArquivo: (origem, destino) => {
    fs.copyFileSync(origem, destino);
    fs.rmSync(origem);
  },
};
