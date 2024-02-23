const fs = require('fs');
const path = require('path');
const crypto = require('crypto');

let root = path.resolve(__dirname);
while (path.basename(root).toLowerCase() != 'nodeservices')
    root = path.resolve(root, '..');
root = path.resolve(root, 'src')

const cacheFile = root + '/db.json';
const cacheFolder = root + '/mp3/cache';

module.exports = function Cache() {
    const fileExists = fs.existsSync(cacheFile) || fs.writeFileSync(cacheFile, '{}');
    const folderExists = fs.existsSync(cacheFolder) || fs.mkdirSync(cacheFolder, { recursive: true });

    this.content = JSON.parse(fs.readFileSync(cacheFile).toString());

    this.savePhrase = function (phrase, audio) {
        phrase = phrase.trim();
        const hex = crypto.randomBytes(6).toString('hex');
        fs.copyFileSync(audio, `${cacheFolder}/${hex}.mp3`);

        this.content[phrase] = `${cacheFolder}/${hex}.mp3`;
        fs.writeFileSync(cacheFile, JSON.stringify(this.content));
    }

    this.getPhrase = (phrase) => {
        phrase = phrase.trim();
        if (this.content[phrase]) return this.content[phrase];
        return null;
    }
}