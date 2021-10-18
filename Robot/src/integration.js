const _process = require('./services/gerar-video-processo');

(async () => {
    "use strict";
    console.log(process.argv);
    const params = {};
    process.argv.forEach(_ => _.indexOf('=') > -1 ? params[_.split('=')[0]] = _.split('=')[1] : undefined);
    console.log(params);
    
    if(params.command == 'processo') await _process(params.id, params.datasource).Executar();
    if(params.command == 'pasta') await _process(params.id, params.datasource).AbrirPasta();
 
})();

// http://localhost:51203/api/processo/7932f