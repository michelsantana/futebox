const Status = require('./model/statuscodes');
const CommandArguments = require('./model/command-args');

const ImagemService = require('./services/imagem-service');
const ImagemServiceSettings = require('./model/imagem-service-settings');

const AudioService = require('./services/audio-service');
const AudioServiceSettings = require('./model/audio-service-settings');

const BatchService = require('./services/batch-service');
const BatchServiceSettings = require('./model/batch-service-settings');

const UploadService = require('./services/upload-service');
const UploadServiceSettings = require('./model/upload-service-settings');

const ServiceResult = require('./model/service-result');
const ProcessoService = require('./services/processo-service');
const SubProcesso = require('./model/subprocesso');

(async () => {
  'use strict';

  console.log('args', process.argv);
  const params = {};
  process.argv.forEach((_) => (_.indexOf('=') > -1 ? (params[_.split('=')[0]] = _.split('=')[1]) : undefined));
  process.argv.forEach((_) => (_.indexOf('@') > -1 ? (params[_.split('@')[0]] = _.split('@')[1]) : undefined));
  console.log('params', params);
  const args = new CommandArguments(params);
  const _processosService = new ProcessoService(args.api);
  const subprocesso = await _processosService.ObterSubProcesso();

  const Try = (fn) => {
    try {
      return fn();
    } catch (ex) {
      console.log(ex);
    }
  };

  const HandleResultService = (
    /** @type {ServiceResult} */
    resultado
  ) => {
    if (resultado.status == Status.erro) console.log(`!ERROR|${resultado.mensagem}`);
    if (resultado.status == Status.ok) console.log(`!OK`);
    if (resultado.status == Status.authFailed) console.log(`!AUTHFAILED`);
    if (resultado.status == Status.blank) console.log(`!BLANK`);
    if (resultado.status == Status.invalid) console.log(`!INVALID`);
  };

  if (args.command == 'imagem') {
    Try(async () => {
      const settings = new ImagemServiceSettings(params);
      const _service = new ImagemService(settings);
      const resultado = await _service.Executar();
      HandleResultService(resultado);
    });
  }
  if (args.command == 'audio') {
    Try(async () => {
      const settings = new AudioServiceSettings(subprocesso, params);
      const _service = new AudioService(settings);
      const resultado = await _service.Executar();
      HandleResultService(resultado);
    });
  }
  if (args.command == 'video') {
    Try(async () => {
      const settings = new BatchServiceSettings(subprocesso, params);
      const _service = new BatchService(settings);
      _service.GerarArquivosDeAtributosDoVideo();
      _service.GerarBatchFFMPEG();
      const resultado = _service.BatchGerarVideo();
      HandleResultService(resultado);
    });
  }
  if (args.command == 'publicar') {
    Try(async () => {
      const settings = new UploadServiceSettings(subprocesso, params);
      const _service = new UploadService(settings);
      const resultado = await _service.Executar();
      HandleResultService(resultado);
    });
  }
})();
