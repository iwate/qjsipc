using HostMethod;
using QjsIpc;

await using var engine = new QjsIpcEngine();

engine.Start(new QjsIpcOptions
{
    ScriptFileName = "main.js",
    AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
    MethodsHost = new Host()
});

var message = await engine.InvokeAsync<string>("transform", "iwate");

Console.WriteLine(message);