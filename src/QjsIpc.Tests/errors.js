import * as ipc from 'ipc'

ipc.register('willRejectString', () => {
    return Promise.reject('some error');
})

ipc.register('willRejectObject', () => {
    return Promise.reject({
        message: 'some error'
    });
})

ipc.register('missingfunc', () => {
    invoke_missing_func();
})

ipc.listen();