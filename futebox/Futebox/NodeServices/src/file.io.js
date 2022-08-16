// const fs = require("fs");
// const path = require("path");
// let _root = '';

// const makeDir = (dir) => {
//     if (!fs.existsSync(pathTo(dir))) {
//         fs.mkdirSync(pathTo(dir), { recursive: true });
//     }
// };

// const makeFile = (file, content) => {
//     if (!fs.existsSync(pathTo(file))) {
//         fs.writeFileSync(pathTo(file), content);
//     }
// };

// const writeFile = (file, content, encoding = null) => {
//     fs.writeFileSync(pathTo(file), content, encoding);
// };

// const readFile = (file) => {
//     return fs.readFileSync(pathTo(file)).toString();
// };

// const pathTo = (relative = './') => {
//     if (!_root) {
//         _root = path.resolve('./');
//         while (path.basename(_root).toLowerCase() != 'nodeservices')
//             _root = path.resolve(_root, '..');
//         _root = path.resolve(_root, 'src')
//     }
//     return path.resolve(_root, relative);
// };

// const copyFile = (source, target) => {
//     let targetFile = target;
//     if (fs.existsSync(target)) {
//         if (fs.lstatSync(target).isDirectory()) {
//             targetFile = path.join(target, path.basename(source));
//         }
//     }
//     fs.writeFileSync(targetFile, fs.readFileSync(source));
// }

// module.exports = {
//     makeDir,
//     makeFile,
//     readFile,
//     pathTo,
//     copyFile,
//     writeFile
// }