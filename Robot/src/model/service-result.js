const Status = require('./statuscodes');

module.exports = class ServiceResult {
    status = Status.blank;
    mensagem = '';
    resultado = '';
    stack = [];
    constructor(status, mensagem, resultado = '') {
      this.status = status;
      this.mensagem = mensagem;
      this.resultado = resultado;
    }
    log = (m) => `${new Date()} - ${this.stack.push(m)}`;
  };
  