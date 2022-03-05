using BenchmarkDotNet.Attributes;
using QjsIpc;

namespace Benchmark;
public class Host
{
    public string GetHostValue() => "host";
}
public class Bench
{
    private QjsIpcEngine? _engine;

    [GlobalSetup]
    public void Setup()
    {
        _engine = new QjsIpcEngine();
        _engine.Start(new QjsIpcOptions
        {
            ScriptFileName = "main.js",
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            MethodsHost = new Host()
        });
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _engine!.DisposeAsync();
    }

    [Benchmark]
    public async Task<string?> Echo() => await _engine!.InvokeAsync<string>("echo", "Hello, World!");

    [Benchmark]
    public async Task<string?> HostValue() => await _engine!.InvokeAsync<string>("hostValue");

    [Benchmark]
    public async Task<string?> Ejs() => await _engine!.InvokeAsync<string>("ejs", new { name = "iwate" });

    [Benchmark]
    public async Task<string?> EjsWithHostValue() => await _engine!.InvokeAsync<string>("ejsWithHostValue", new { name = "iwate" });
}
