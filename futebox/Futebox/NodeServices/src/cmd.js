const path = require('path');
const exec = require('child_process').execSync;

module.exports = function (callback, command = `explorer ${__dirname}`) {
    (async () => {
        const result = { status: true, inputParams: [command], output: [], steps: [], error: null };
        const logger = {
            log: (a, b) => {
                result.steps.push(a);
                if (b) result.steps.push(JSON.stringify(b));
                console.log(a, b ?? '');
            }
        };
        try {

            

            let root = path.resolve(__dirname);
            while (path.basename(root).toLowerCase() != 'nodeservices') root = path.resolve(root, '..');
            root = path.resolve(root, 'src');

            logger.log('starting');
            var e = exec(command);
            console.log(e);
            result.output.push(true);

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
