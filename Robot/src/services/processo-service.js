const axios = require("axios").default;

module.exports = function (datasourceUrl) {
  this.ObterProcesso = async () => {
    return (await axios.get(`${datasourceUrl}/obter`)).data;
  };

  this.AtualizarProcessoErro = async (erro) => {
    return (await axios.get(`${datasourceUrl}/erro?mensagem=${erro}`)).data;
  };

  this.AtualizarProcessoSucesso = async () => {
    return (await axios.get(`${datasourceUrl}/sucesso`)).data;
  };
  return this;
};
