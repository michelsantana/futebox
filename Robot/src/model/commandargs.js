module.exports = class CommandArguments {
  command = null;
  id = null;
  datasource = null;
  constructor(params) {
    params = params || {};
    this.command = params.command;
    this.id = params.id;
    this.datasource = params.datasource;
  }
};
