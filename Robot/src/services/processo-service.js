const axios = require('axios').default;

module.exports = function (datasourceUrl) {
  this.ObterProcesso = async () => (await axios.get(`${datasourceUrl}/obter`)).data;

  this.AtualizarProcessoErro = async (erro) => (await axios.get(`${datasourceUrl}/erro?mensagem=${erro}`)).data;

  this.AtualizarProcessoSucesso = async (arquivo) => (await axios.get(`${datasourceUrl}/sucesso?arquivo=${encodeURIComponent(arquivo)}`)).data;

  this.AtualizarLogProcesso = async (message) => (await axios.post(`${datasourceUrl}/log`, { log: message })).data;

  return this;
};
