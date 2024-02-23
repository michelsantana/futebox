const UploadServiceSettings = require('../model/upload-service-settings');
const UploadInstagramService = require('./upload-instagram-service');
const UploadYoutubeService = require('./upload-youtube-service');
require('dotenv').config();

module.exports = class UploadService {
  /** @type {UploadServiceSettings} */
  settings;

  constructor(settings = new Settings()) {
    this.settings = settings;
  }

  async Executar() {
    try {
      if (this.settings.redeSocial == 'YoutubeShorts') {
        const _service = new UploadYoutubeService(this.settings);
        return _service.Executar();
      }
      if (this.settings.redeSocial == 'YoutubeVideo') {
        const _service = new UploadYoutubeService(this.settings);
        return _service.Executar();
      }
      if (this.settings.redeSocial == 'InstagramVideo') {
        const _service = new UploadInstagramService(this.settings);
        return _service.Executar();
      }
    } catch (ex) {
      throw ex;
    }
  }

  async StatusLogin() {
    try {
      if (this.settings.redeSocial == 'YoutubeShorts') {
        const _service = new UploadYoutubeService(this.settings);
        return _service.StatusLogin();
      }
      if (this.settings.redeSocial == 'YoutubeVideo') {
        const _service = new UploadYoutubeService(this.settings);
        return _service.StatusLogin();
      }
      if (this.settings.redeSocial == 'InstagramVideo') {
        const _service = new UploadInstagramService(this.settings);
        return _service.StatusLogin();
      }
    } catch (ex) {
      throw ex;
    }
  }
};
