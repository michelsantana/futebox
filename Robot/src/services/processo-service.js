const axios = require("axios").default;

module.exports = function (datasourceUrl) {
  this.ObterProcesso = async () => {
    return (await axios.get(`${datasourceUrl}/obter`)).data;
  };

  this.AtualizarProcessoErro = async (erro) => {
    return (await axios.get(`${datasourceUrl}/erro?mensagem=${erro}`)).data;
  };

  this.AtualizarProcessoSucesso = async (arquivo) => {
    return (await axios.get(`${datasourceUrl}/sucesso?arquivo=${encodeURIComponent(arquivo)}`)).data;
  };

  this.AtualizarLogProcesso = async (message) =>{
    return (await axios.post(`${datasourceUrl}/log`, { log: message })).data;
  }
  return this;
};
