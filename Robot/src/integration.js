const _process = require('./services/gerar-video-processo');
const _upload = require('./services/upload-youtube');
const path = require('path');

(async () => {
    "use strict";
    
    console.log('args', process.argv);
    const params = {};
    process.argv.forEach(_ => _.indexOf('=') > -1 ? params[_.split('=')[0]] = _.split('=')[1] : undefined);
    console.log(params);

    if(params.command == 'executar') await _process(params.id, params.datasource).Executar();
    if(params.command == 'publicar') await _upload(params.id, params.datasource).Executar();
    if(params.command == 'pasta') await _process(params.id, null).AbrirPasta();
    if(params.command == 'test') {
        //await _upload(null, null).Executar();
        console.log(path.resolve('./'));
    }
 
})();

// http://localhost:51203/api/processo/7932f