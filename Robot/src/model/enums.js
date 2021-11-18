module.exports = {
  TipoLink: class TipoLink {
    tipo = {
      code: -1,
      desc: null,
    };

    static print = {
      code: 0,
      desc: 'print',
    };

    static image = {
      code: 1,
      desc: 'image',
    };

    constructor(code) {
      if (code == TipoLink.print.code) {
        this.code = TipoLink.print.code;
        this.desc = TipoLink.print.desc;
      }
      if (code == TipoLink.image.code) {
        this.code = TipoLink.image.code;
        this.desc = TipoLink.image.desc;
      }
    }
  },
  TipoProcesso: class TipoProcesso {
    code = -1;
    desc = null;

    static partida = {
      code: 0,
      desc: 'partida',
    };

    static classificacao = {
      code: 1,
      desc: 'classificacao',
    };

    static rodada = {
      code: 2,
      desc: 'rodada',
    };

    constructor(code) {
      if (code == TipoProcesso.partida.code) {
        this.code = TipoProcesso.partida.code;
        this.desc = TipoProcesso.partida.desc;
      }
      if (code == TipoProcesso.classificacao.code) {
        this.code = TipoProcesso.classificacao.code;
        this.desc = TipoProcesso.classificacao.desc;
      }
      if (code == TipoProcesso.rodada.code) {
        this.code = TipoProcesso.rodada.code;
        this.desc = TipoProcesso.rodada.desc;
      }
    }
  },
};
