import * as ipc from 'ipc';

ipc.register('echo', function (payload) {
  return payload;
});

ipc.listen();
