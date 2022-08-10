function YTSelecionarPlaylist(nomePlaylist) {

    const enumNomePlaylist = { short: 'shorts', classificacao: 'classi', rodada: 'calen' };
    nomePlaylist = enumNomePlaylist[nomePlaylist];

    let id = '';
    const seletorLista = '.checkbox-label.style-scope.ytcp-checkbox-group';
    const seletorNomeDoItem = '.label.label-text.style-scope.ytcp-checkbox-group';
    const itens = document.querySelectorAll(seletorLista);

    Array.from(itens).forEach((_) => {
        const item = _.querySelector(seletorNomeDoItem)?.innerHTML;
        if (item && item.toLowerCase()?.indexOf(nomePlaylist.toLowerCase()) > -1) id = _.id;
    });
    return id;
}

function YTEstaLogado() {
    return document.querySelectorAll('[type="email"]').length == 0;
}

function IGEstaLogado() {
    return document.querySelectorAll('[aria-label="Nova publicação"]').length > 0;
}

function IGNaoPermiteNotificacao() {
    try {
        document.querySelectorAll('[role="dialog"] [tabindex]')[1].click()
    } catch (e) {}
}

function IGSelecionarDimensoes() {
    document.querySelector('[aria-label="Selecionar corte"]').parentElement.parentElement.click();
    setTimeout(() => {
        Array.from(document.querySelectorAll('[type=button]'))
            .filter((_) => _.innerHTML.indexOf('>Original<') > -1)
            .forEach((_) => _.click());
    }, 1000);
}

function IGClicarEmProximo() {
    document.querySelectorAll('[role="dialog"] button').forEach((_) => {
        if (_.innerHTML.toLowerCase().indexOf('avan') > -1) _.click();
    });
}

function IGAtivarLegendasAutomaticas() {
    document.querySelectorAll('[role="button"]').forEach((_) => {
        if (_.innerHTML.toLowerCase().indexOf('acessi') > -1) _.click();
    });

    setTimeout(() => {
        document.querySelectorAll('label > input').forEach((_) => {
            if (!_.checked && _.parentElement.parentElement.innerHTML.toLowerCase().indexOf('legendas geradas') > -1) _.click();
        });
    }, 1000);
}

function IGPublicar() {
    document.querySelectorAll('[role="dialog"] button').forEach((_) => {
        if (_.innerHTML.toLowerCase().indexOf('compart') > -1) _.click();
    });
}

function IBMInjetarScriptExtrairAudio(nomeDoArquivoDownload) {

    let endlink = document.createElement('button');
    endlink.innerHTML = 'FinalizarForçado';
    endlink.setAttribute('class', 'dwlend');
    endlink.setAttribute('onclick', 'this.id="dwlend"');
    endlink.setAttribute('style', `position:absolute;top:150px;left:0;z-index:99999999999999;font-size:50px;background:#F00;width:100%;`);
    document.body.append(endlink);

    var audio = document.querySelector('audio');
    audio.onplay = function () {
        let link = document.createElement('a');
        link.innerHTML = 'IndicadorDeInicialização';
        link.setAttribute(`href`, audio.src);
        link.setAttribute(`download`, nomeDoArquivoDownload);
        link.setAttribute('id', 'dwl');
        link.setAttribute('style', `position:absolute;top:50px;left:0;z-index:99999999999999;font-size:50px;background:#F00;width:100%;`);
        document.body.append(link);
    };

    audio.onended = function () {
        let link = document.createElement('a');
        link.innerHTML = 'IndicadorDeFinalização';
        link.setAttribute(`href`, '#');
        link.setAttribute('id', 'dwlend');
        link.setAttribute('style', `position:absolute;top:100px;left:0;z-index:99999999999999;font-size:50px;background:#F00;width:100%;`);
        document.body.append(link);
    };
}

function IBMForcarFinalizarDownload() {
    document.querySelector('.dwlend').id = 'dwlend';
}

