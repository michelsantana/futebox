const fs = require("fs");
const path = require("path");
const googleTTS = require('google-tts-api'); // CommonJS
const exec = require('child_process').execSync;
const crypto = require('crypto');
const Cache = require('./cache');
const cache = new Cache();
const message = fs.readFileSync(`${__dirname}/sample/message.txt`).toString();

module.exports = function (
    callback,
    processoId = '010101',
    roteiro = message,
    nomeDoArquivoDeAudio = "C:/Users/Michel/Downloads/010101.mp3"
) {
    (async () => {
        const result = {
            status: true,
            inputParams: [processoId, roteiro, nomeDoArquivoDeAudio],
            output: [],
            steps: [],
            error: null,
        };

        const logger = {
            log: (a, b) => {
                result.steps.push(a);
                if (b) result.steps.push(JSON.stringify(b));
                console.log(a, b ?? '');
            }
        };

        let root = path.resolve(__dirname);
        while (path.basename(root).toLowerCase() != 'nodeservices')
            root = path.resolve(root, '..');
        root = path.resolve(root, 'src')

        try {

            logger.log('starting');

            const uuid = processoId;
            const uuidPath = path.resolve(`${root}/mp3/${uuid}`);
            const resultsPath = `${nomeDoArquivoDeAudio}`;

            const message = roteiro;
            const messageSplited = message.split(':');
            logger.log('params - ok');

            if (messageSplited.some(_ => _.length > 200)) {
                throw new Error("Roteiro com parte muito grande!");
            }
            logger.log('validation - ok');

            const dir = fs.existsSync(uuidPath) ? null : fs.mkdirSync(uuidPath, { recursive: true });

            logger.log('folder - ok');

            const mergeMp3FilesWithFFMPEG = async (files) => {
                logger.log('merging files');

                const slowFile = path.resolve(`${uuidPath}/slow.mp3`);
                if (fs.existsSync(slowFile)) fs.rmSync(slowFile);

                const endFile = resultsPath;
                if (fs.existsSync(endFile)) fs.rmSync(endFile);

                const commandMerge = `ffmpeg -i "concat:${files.join('|')}" -c copy -map 0:0 -f mp3 ${slowFile}`;
                const commandSpeedup = `ffmpeg -i "${uuidPath}/slow.mp3" -filter:a atempo=1.56 "${endFile}"`;

                console.log(commandMerge);
                exec(commandMerge);

                console.log(commandSpeedup);
                exec(commandSpeedup);
            }

            const gerarAudio = async (msg, file) => {
                logger.log('generating new audio', msg, file);

                await googleTTS
                    .getAudioBase64(msg, {
                        lang: 'pt',
                        slow: false,
                        host: 'https://translate.google.com',
                        timeout: 10000
                    })
                    .then(_ => {
                        logger.log('saving new audio');
                        fs.writeFileSync(file, _, 'base64');

                        logger.log('caching new audio');
                        cache.savePhrase(msg, file);
                    })
                    .catch((err) => { throw err });
            }

            let index = 0;
            const files = [];
            for (const phrase of messageSplited) {
                index++;

                logger.log('processing phrase', index, phrase);

                const file = path.resolve(`${uuidPath}/${index}.mp3`);
                const audioFile = cache.getPhrase(phrase);

                if (audioFile) {
                    logger.log('using cached phrase');
                    fs.copyFileSync(audioFile, file);
                }
                else await gerarAudio(phrase, file);

                files.push(file);
            }

            await mergeMp3FilesWithFFMPEG(files);
            result.output.push(resultsPath)

        } catch (ex) {
            logger.log(`an error occured`);
            logger.log(ex?.message);
            result.error = JSON.stringify(ex);
            result.status = false;
        }

        return callback(/* error */ null, result);
    })();
};

//module.expor
