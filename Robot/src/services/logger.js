const moment = require('moment');

module.exports = function Logger(UID) {
  this.stack = [];
  this.log = function(m) {
    const now = moment().format("DD/MM/YYYY HH:mm:ss");
    const message = `[${UID}][${now}]\n\t${m}`;
    this.stack.push(message);
    console.log(message);
  };

  return this;
};
