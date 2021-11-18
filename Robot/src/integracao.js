const CommandArguments = require('./model/commandargs');
const Settings = require('./model/settings');
const Processo = require('./model/processomodel');
const ServiceResult = require('./model/serviceresult');
const Status = require('./model/statuscodes');

const AudioService = require('./services/audio-service');
const ProcessoService = require('./services/processo-service');
const ImagemService = require('./services/imagem-service');
const BatchService = require('./services/batch-service');
const UploadService = require('./services/upload-service');

const pastas = require('./utils/gerenciador-pastas');

module.exports = class Integracao {
  /** @type {CommandArguments} */
  args = new CommandArguments();

  constructor(args = new CommandArguments()) {
    this.args = args;
  }

  async #GerarVideo() {
    const pasta = `${pastas.obterPastaArquivosDoDia()}/${this.args.id}`;
    const _processoService = new ProcessoService(this.args.datasource);
    const processo = await _processoService.ObterProcesso();

    try {
      const settings = new Settings(pasta, `${this.args.id}`, processo, true);

      const _imagemService = new ImagemService(settings);
      const resultadoImagem = await _imagemService.Executar();
      const arquivoImagemSplited = resultadoImagem.arg.split('|');
      const arquivoImagem = arquivoImagemSplited[0];

      const _audioService = new AudioService(settings);
      const resultadoAudio = await _audioService.Executar();
      const arquivoAudio = resultadoAudio.arg;

      const _batchService = new BatchService(settings);
      _batchService.GerarArquivosDeAtributosDoVideo();
      _batchService.GerarBatchFFMPEG(arquivoImagem, arquivoAudio);
      const resultadoBatch = _batchService.BatchGerarVideo();

      await _processoService.AtualizarProcessoSucesso(resultadoBatch.arg);
      return new ServiceResult(Status.ok, 'Video concluido');
    } catch (ex) {
      console.log(ex);
      await _processoService.AtualizarProcessoErro('');
      return new ServiceResult(Status.erro, 'Erro ao gerar video');
    }
  }
  async #UploadVideo() {
    const pasta = `${pastas.obterPastaArquivosDoDia()}/${this.args.id}`;
    const _processoService = new ProcessoService(this.args.datasource);
    const processo = await _processoService.ObterProcesso();
    try {
      const settings = new Settings(pasta, `${this.args.id}`, processo, true);
      const _uploadService = new UploadService(settings);
      const resultadoUpload = await _uploadService.Executar();
      if (resultadoUpload.status == Status.erro) throw new Error(resultadoUpload.arg);

      return resultadoUpload;
    } catch (ex) {
      console.log(ex);
      await _processoService.AtualizarProcessoErro('');
      return new ServiceResult(Status.erro, 'Erro ao publicar video');
    }
  }
  async #StatusLoginYoutube() {
    const settings = new Settings('', `youtube`, {}, true);
    const _uploadService = new UploadService(settings);
    const resultadoUpload = await _uploadService.StatusLogin();
    return resultadoUpload;
  }
  async #AbrirPastaArquivos() {
    const _batchService = new BatchService(new Settings(pasta, `${uid}`, processo, true));
    _batchService.BatchAbrirPasta();
    return new ServiceResult(Status.ok, 'Pasta aberta');
  }
  async Executar() {
    if (this.args.command == 'video') return await this.#GerarVideo();
    else if (this.args.command == 'publicar') return await this.#UploadVideo();
    else if (this.args.command == 'youtube') return await this.#StatusLoginYoutube();
    else if (this.args.command == 'pasta') return await this.#AbrirPastaArquivos();
    return new ServiceResult(Status.invalid, 'comando invalido');
  }
};
