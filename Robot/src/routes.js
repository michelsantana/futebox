const express = require('express');
const routes = express.Router();

const ImagemService = require('./services/imagem-service');
const ImagemServiceSettings = require('./model/imagem-service-settings');

const AudioService = require('./services/audio-service');
const AudioServiceSettings = require('./model/audio-service-settings');

const BatchService = require('./services/batch-service');
const BatchServiceSettings = require('./model/batch-service-settings');

const UploadService = require('./services/upload-service');
const UploadServiceSettings = require('./model/upload-service-settings');

const SubProcesso = require('./model/subprocesso');
const ServiceResult = require('./model/service-result');
const Status = require('./model/statuscodes');

routes.get('/', async (req, res) => res.json({ status: 'ok' }));

routes.post('/rc/imagem', async (req, res) => {
  try {
    const subprocesso = new SubProcesso(req.body);
    const settings = new ImagemServiceSettings(subprocesso);
    const _service = new ImagemService(settings);
    const resultado = await _service.Executar();
    return res.json(resultado);
  } catch (ex) {
    return new ServiceResult(Status.erro, 'erro inesperado', JSON.stringify(ex));
  }
});

routes.post('/rc/audio', async (req, res) => {
  try {
    const subprocesso = new SubProcesso(req.body);
    const settings = new AudioServiceSettings(subprocesso);
    const _service = new AudioService(settings);
    const resultado = await _service.Executar();
    return res.json(resultado);
  } catch (ex) {
    return new ServiceResult(Status.erro, 'erro inesperado', JSON.stringify(ex));
  }
});

routes.post('/rc/video', async (req, res) => {
  try {
    const subprocesso = new SubProcesso(req.body);
    const settings = new BatchServiceSettings(subprocesso);
    const _service = new BatchService(settings);
    _service.GerarArquivosDeAtributosDoVideo();
    _service.GerarBatchFFMPEG(settings);
    const resultado = _service.BatchGerarVideo();
    return res.json(resultado);
  } catch (ex) {
    return new ServiceResult(Status.erro, 'erro inesperado', JSON.stringify(ex));
  }
});

routes.post('/rc/publicar', async (req, res) => {
  try {
    const subprocesso = new SubProcesso(req.body);
    const settings = new UploadServiceSettings(subprocesso);
    const _service = new UploadService(settings);
    const resultado = await _service.Executar();
    return res.json(resultado);
  } catch (ex) {
    return new ServiceResult(Status.erro, 'erro inesperado', JSON.stringify(ex));
  }
});


routes.get('/rc/pasta', async (req, res) => {
  try {
    const { pasta } = req?.query;
    const resultado = BatchService.BatchAbrirPasta(decodeURIComponent(pasta));
    return res.json(resultado);
  } catch (ex) {
    return new ServiceResult(Status.erro, 'erro inesperado', JSON.stringify(ex));
  }
});

routes.get('/rc/ytstatus', async (req, res) => {
  const _service = new UploadService({ redeSocial: 'YoutubeShorts' });
  const resultado = await _service.StatusLogin();
  return res.json(resultado);
});

routes.get('/rc/igstatus', async (req, res) => {
  const _service = new UploadService({ redeSocial: 'InstagramVideo' });
  const resultado = await _service.StatusLogin();
  return res.json(resultado);
});

module.exports = routes;
