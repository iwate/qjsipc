using QjsIpc;

await using var engine = new QjsIpcEngine();

engine.Start(new QjsIpcOptions
{
    ScriptFileName = "main.js",
    AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
});

var message = await engine.InvokeAsync<string>("echo", "Hello, World!");

Console.WriteLine(message);