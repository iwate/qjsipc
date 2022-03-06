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


`register: (name: string, procedure: (...params: any[]) => Promise<any> | any) => void`  
Register a JavaScript procedure.


`invoke: (name: string, ...params: any[]) => Promise<any>`  
Invoke a .NET host procedure.

`listen: () => void`  
Start communication to .NET host.

### Benchmark

```
// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
11th Gen Intel Core i7-1165G7 2.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.200-preview.21617.4
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


|           Method |       Mean |     Error |    StdDev |
|----------------- |-----------:|----------:|----------:|
|             Echo |   303.7 us |  28.22 us |  81.42 us |
|        HostValue |   456.2 us |  41.25 us | 120.97 us |
|              Ejs | 1,414.2 us |  91.87 us | 269.43 us |
| EjsWithHostValue | 1,499.9 us | 121.33 us | 340.22 us |
```


### LICENSE

This library bundle modified QuickJS and qjs.wasm source code.

- [QuickJS License](https://github.com/bellard/quickjs/blob/master/LICENSE)
- [qjs.wasm License](https://github.com/saghul/wasi-lab#license)
