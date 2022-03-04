import * as ipc from 'ipc';
import * as std from 'std';
import ejs from './ejs.mjs';
ejs.fileLoader = function (filename) {
  const file = std.open(filename, 'r');
  return file.readAsString();
};

ipc.register('ejs', function (payload) {
  return new Promise(function (resolve, reject) {
    ejs.renderFile('template.ejs', payload, {}, function (err, html) {
      if (err) {
        reject(err);
      }
      else {
        resolve(html);
      }
    });
  });
});

ipc.listen();

