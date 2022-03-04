using QjsIpc;

await using var engine = new QjsIpcEngine();

engine.Start(new QjsIpcOptions
{
    ScriptFileName = "main.js",
    AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
});

var message = await engine.InvokeAsync<string>("ejs", new { name = "iwate" });

Console.WriteLine(message);