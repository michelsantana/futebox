module.exports = class CommandArguments {
  command = null;
  api = null;
  constructor(params) {
    params = params || {};
    this.command = params.command;
    this.api = params.api;
  }
};
