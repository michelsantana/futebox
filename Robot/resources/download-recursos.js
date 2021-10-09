const axios = require("axios").default;
const fs = require("fs");

var baixarArquivo = async (url, pasta) => {
  const writer = fs.createWriteStream(pasta);

  const response = await axios({
    url,
    method: "GET",
    responseType: "stream",
  });

  response.data.pipe(writer);

  return new Promise((resolve, reject) => {
    writer.on("finish", resolve);
    writer.on("error", reject);
  });
};
var tryit = async (fn, ca) => {
  try {
    await fn();
  } catch (e) {
    await ca(e);
  }
};

var traduzirNomes = (nome) => {
  if (nome.indexOf("-mg")) return nome.replace("-mineiro");
  if (nome.indexOf("-pr")) return nome.replace("-paranaense");
  if (nome.indexOf("-go")) return nome.replace("-goianiense");
};

var json = [
  "flamengo",
  "atletico-mg",
  "sao-paulo",
  "palmeiras",
  "river-plate",
  "barcelona-equ",
  "internacional",
  "boca-juniors",
  "independiente-del-valle",
  "velez-sarsfield",
  "universidad-catolica",
  "defensa-y-justicia",
  "cerro-porteno",
  "fluminense",
  "olimpia-par",
  "santos",
  "argentinos-juniors",
  "racing-club",
  "america-de-cali",
  "junior-barranquilla",
  "atletico-nacional",
  "sporting-cristal",
  "santa-fe",
  "union-la-calera",
  "the-strongest",
  "ldu-quito",
  "club-always-ready",
  "la-guaira",
  "deportivo-tachira",
  "rentistas",
  "universitario",
  "nacional-uru",
];
json = Array.from(json).map((_) => traduzirNomes(_));

(async () => {
  var grupos = [];
  json.forEach((_) => {
    var links = [];
    links.push({
      link: `https://logodetimes.com/times/${_}/logo-${_}-4096.png`,
      nome: _,
    });
    links.push({
      link: `https://logodetimes.com/times/${_}/logo-${_}-2048.png`,
      nome: _,
    });
    links.push({
      link: `https://logodetimes.com/times/${_}/logo-${_}-1536.png`,
      nome: _,
    });
    links.push({
      link: `https://logodetimes.com/times/${_}/logo-${_}-1024.png`,
      nome: _,
    });
    links.push({
      link: `https://logodetimes.com/times/${_}/logo-${_}-512.png`,
      nome: _,
    });
    links.push({
      link: `https://logodetimes.com/times/${_}/logo-${_}-256.png`,
      nome: _,
    });
    grupos.push(links);
  });

  for (var grupo of grupos) {
    var download = false;
    for (var time of grupo) {
      await tryit(
        async () => {
          await baixarArquivo(time.link, `./times/${time.nome}.png`);
          download = true;
        },
        async (ex) => {
          download = false;
        }
      );
    }
  }
})();
