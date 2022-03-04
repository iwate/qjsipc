import * as ipc from 'ipc';

ipc.register('transform', function (name) {
  return new Promise(async function (resolve, reject) {
    try {
      const value1 = await ipc.invoke('GetValue', 'Key1');
      const value2 = await ipc.invoke('GetValueTask', 'Key2');
      const value3 = await ipc.invoke('GetValueAsync', 'Key3');
      resolve(`Hi! ${name}, ${value1} ${value2} ${value3}`);
    }
    catch (err) {
      reject(err);
    }
  })
});

ipc.listen();
