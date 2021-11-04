const axios = require("axios");
const processoServiceContructor = require("./processo-service");
const Processo = require("./../model/processo");

const botToken = ""; 
const chatId = ""; //Id do usuario
const baseUrl = "https://api.telegram.org/" + `${botToken}`;
const messageSize = 1024;

// Migrado para o backend C#
module.exports = function (processo, datasourceUrl) {
  const UID = processo;
  const processoService = new processoServiceContructor(datasourceUrl);

  this.ObterProcesso = async () => {
      throw new Error("Sou obsoleto! deve chamar NotifyService do backend C#");
      return (await processoService.ObterProcesso());
  };

  this.Notificar = async () => {
      throw new Error("Sou obsoleto! deve chamar NotifyService do backend C#");
    const processoData = await this.ObterProcesso();

    const processo = new Processo(processoData);
    let message = `${processo.notificacao}`;
    if (message.length > messageSize) message = message.substr(0, messageSize);
    message = encodeURI(message);
    const result = await axios.get(
      `${baseUrl}/sendMessage?chat_id=${chatId}&text=${message}`
    );
    console.log(result);
  };

  return this;
};

