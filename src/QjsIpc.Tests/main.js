import * as ipc from 'ipc';

ipc.register('echo', function (payload) {
  return payload;
});

ipc.register('transform', function (payload) {
  return ipc.invoke('Echo', 'Hello, World').then(function (message) {
    payload.message = message;
    return payload;
  });
});

ipc.listen();
