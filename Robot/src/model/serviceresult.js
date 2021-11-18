const Status = require('./statuscodes');

module.exports = class ServiceResult {
    status = Status.blank;
    mensagem = '';
    arg = '';
    constructor(status, mensagem, arg = '') {
      this.status = status;
      this.mensagem = mensagem;
      this.arg = arg;
    }
  };
  