const fs = require("fs");
const path = require("path");
const jimp = require("jimp");

module.exports = function (
  callback,
  imageInput,
  maxWidth,
  maxHeight,
  applyMargin
) {
  (async () => {
    const result = {
      status: true,
      inputParams: [imageInput, maxWidth, maxHeight, applyMargin],
      output: [],
      steps: [],
      error: null,
    };
    const log = (m) => result.steps.push(m);

    try {
      log("loading image");
      const originalImage = await jimp.read(imageInput);

      const outputFolder = path.dirname(imageInput);
      const outputFile = 'crop-image.png';

      log(`cloning image`);
      let image = await originalImage.clone();

      log(`looking for aspect ratio`);
      const portrait = image.bitmap.height > image.bitmap.width * 1.5;

      if (portrait) {
        log(`resizing image to portrait`);
        image = await image.resize(jimp.AUTO, maxHeight);
        const w = image.bitmap.width;
        while (image.bitmap.width > maxWidth) {
          image = await image.resize(image.bitmap.width - w * 0.05, jimp.AUTO);
        }
      } else {
        log(`resizing image to landscape`);
        image = await image.resize(maxWidth, jimp.AUTO);
        const h = image.bitmap.height;
        while (image.bitmap.height > maxHeight) {
          image = await image.resize(jimp.AUTO, image.bitmap.height - h * 0.05);
        }
      }

      if (applyMargin) {
        log(`appling margin`);
        image = await image.resize(image.bitmap.width * 0.85, jimp.AUTO);
      }

      log(`appling blur`);
      let bg = await image.clone();
      bg = await bg.resize(maxWidth, maxHeight);
      bg = await bg.blur(50);

      log(`merging layers`);
      let x = maxWidth / 2 - image.bitmap.width / 2;
      let y = maxHeight / 2 - image.bitmap.height / 2;
      image = bg.blit(image, x, y);
      log(`saving output file`);
      const writed = await image.writeAsync(`${outputFolder}/${outputFile}`);
      result.output.push(`${outputFolder}/${outputFile}`);

    } catch (ex) {
      log(`an error occured`);
      log(ex?.message);
      result.error = JSON.stringify(ex);
      result.status = false;
    }

    callback(/* error */ null, JSON.stringify(result));
  })();
};

//module.expor
