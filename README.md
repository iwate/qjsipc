[![Unit Tests](https://github.com/iwate/qjsipc/actions/workflows/test.yml/badge.svg)](https://github.com/iwate/qjsipc/actions/workflows/test.yml)
[![NuGet version](https://badge.fury.io/nu/QjsIpc.svg)](https://badge.fury.io/nu/QjsIpc)
# QjsIpc
A dotnet lib for internal procedure call to [QuickJS](https://bellard.org/quickjs/) wasm runtime.

## How to Use

In JavaScript, you can use `ipc` module for communication to dotnet host.

```js:main.js
import * as ipc from 'ipc';

ipc.register('echo', function (payload) {
  return payload;
});

ipc.listen();
```

In .NET, you can invoke a method which is registered in JavaScript.

```csharp:Program.cs
using QjsIpc;

await using var engine = new QjsIpcEngine();

engine.Start(new QjsIpcOptions
{
    ScriptFileName = "main.js",
    AllowedDirectoryPath = Environment.CurrentDirectory,
});

var message = await engine.InvokeAsync<string>("echo", "Hello, World!");

Console.WriteLine(message);
```

[more examples](https://github.com/iwate/qjsipc/tree/main/examples)

### IPC module

`ipc` module has three public methods.


`register: (name: string) => (...params: any[]) => Promise<any> | any`  
Register a JavaScript procedure.


`invoke: (name: string, ...params: any[]) => Promise<any>`  
Invoke a .NET host procedure.

`listen: () => void`  
Start communication to .NET host.


### LICENSE

This library bundle modified QuickJS and qjs.wasm source code.

- [QuickJS License](https://github.com/bellard/quickjs/blob/master/LICENSE)
- [qjs.wasm License](https://github.com/saghul/wasi-lab#license)
