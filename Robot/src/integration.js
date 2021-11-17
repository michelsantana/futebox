const GerarVideoProcesso = require('./services/gerar-video-processo');
const UploadVideo = require('./services/upload-youtube');

(async () => {
  'use strict';

  const commandReturnResult = (obj) => console.log('!RESULT|' + JSON.stringify(obj));
  const commandReturnTrue = () => console.log('!TRUE');
  const commandReturnFalse = () => console.log('!FALSE');
  const commandReturnError = (msg) => console.log('!ERROR|' + msg);

  console.log('args', process.argv);
  const params = {};
  process.argv.forEach((_) => (_.indexOf('=') > -1 ? (params[_.split('=')[0]] = _.split('=')[1]) : undefined));
  console.log(params);

  const videoProcesso = new GerarVideoProcesso(params.id, params.datasource, logger);
  const uploadVideo = new UploadVideo(params.id, params.datasource, logger);

  await videoProcesso.Executar();
  // if(params.command == 'executar') await _process(params.id, params.datasource).Executar();
  // if(params.command == 'publicar') await _upload(params.id, params.datasource).Executar();
  // if(params.command == 'pasta') await _process(params.id, null).AbrirPasta();
  // if(params.command == 'youtube') console.log('!RESULT FAIL');
  // if(params.command == 'test') {
  //     // await _upload(null, null).Executar();
  //     // console.log(path.resolve('./'));
  // }
})();

// http://localhost:51203/api/processo/7932f
