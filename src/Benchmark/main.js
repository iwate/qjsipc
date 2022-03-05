import * as ipc from 'ipc';
import * as std from 'std';
import ejs from './ejs.mjs';
ejs.fileLoader = function (filename) {
  const file = std.open(filename, 'r');
  if (!file) {
    console.log(`${filename} does not exists.`);
    return '';
  }
  return file.readAsString();
};

function render(template, data) {
  return new Promise(function (resolve, reject) {
    ejs.renderFile(template, data, {}, function (err, html) {
      if (err) {
        console.log(err)
        reject(err);
      }
      else {
        resolve(html);
      }
    });
  });
}

ipc.register('echo', function (payload) {
  return payload;
});

ipc.register('hostValue', async function () {
  const hostValue = await ipc.invoke('GetHostValue');
  return hostValue;
});

ipc.register('ejs', async function (payload) {
  const html = await render('template1.ejs', payload);
  return html;
});

ipc.register('ejsWithHostValue', async function (payload) {
  const hostValue = await ipc.invoke('GetHostValue');
  const html = await render('template2.ejs', Object.assign({ hostValue }, payload));
  return html;
});

ipc.listen();
