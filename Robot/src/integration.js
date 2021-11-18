const Status = require('./model/statuscodes');
const Integracao = require('./integracao');
const CommandArguments = require('./model/commandargs');

(async () => {
  'use strict';

  console.log('args', process.argv);
  const params = {};
  process.argv.forEach((_) => (_.indexOf('=') > -1 ? (params[_.split('=')[0]] = _.split('=')[1]) : undefined));
  console.log('params', params);

  const _integracao = new Integracao(new CommandArguments(params));
  const resultado = await _integracao.Executar();

  if (resultado.status == Status.ok) console.log(`!OK`);
  if (resultado.status == Status.erro) console.log(`!ERROR|${resultado.mensagem}`);
  if (resultado.status == Status.authFailed) console.log(`!AUTHFAILED`);
  if (resultado.status == Status.blank) console.log(`!BLANK`);
  if (resultado.status == Status.invalid) console.log(`!INVALID`);
  
})();
