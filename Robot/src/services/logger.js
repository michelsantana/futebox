// module.exports = function Logger(UID) {
//   this.stack = [];

//   this.log = (service, message) => {
//     const now = moment().format('DD/MM/YYYY HH:mm:ss');
//     const newmessage = `[${UID}][${service}][${now}]\n\t${message}`;
//     this.stack.push(newmessage);
//     console.log(newmessage);
//   };

//   this.return = () => {};

//   return this;
// };

const moment = require('moment');
let instance = new Logger();

class Logger {
  constructor() {
    this.stack = [];
  }

  now() {
    return moment().format('DD/MM/YYYY HH:mm:ss');
  }

  log(UID, service, message) {
    const now = moment().format('DD/MM/YYYY HH:mm:ss');
    const newmessage = `[${UID}][${service}][${now}]\n\t${message}`;
    this.stack.push(newmessage);
    console.log(newmessage);
  }

  static getInstance() {
    if (!instance) instance = new Logger();
    return instance;
  }
}

module.exports = Logger.getInstance();
