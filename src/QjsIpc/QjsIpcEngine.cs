using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wasmtime;
using Module = Wasmtime.Module;
namespace QjsIpc;
public class QjsIpcEngine : IAsyncDisposable
{
    private const string CMD_QUIT = ".quit";
    private ConcurrentWritableBuffer _dotnetBuffer;
    private ConcurrentDictionary<string, JObject> _results;
    private ConcurrentQueue<JObject> _invokes;
    private HostMethodRegistry? _methods;

    public QjsIpcEngine()
    {
        _dotnetBuffer = new ConcurrentWritableBuffer();
        _results = new ConcurrentDictionary<string, JObject>();
        _invokes = new ConcurrentQueue<JObject>();
    }
    private Task? _task = null;
    public void Start(QjsIpcOptions options)
    {
        if (_task != null)
            throw new InvalidOperationException("This instance is aleary running.");

        if (options.MethodsHost != null)
            _methods = HostMethodRegistry.Create(options.MethodsHost);

        var source = new CancellationTokenSource();

        _task = Task.WhenAll(
            Task.Run(() =>
            {
                RunWasm(options);
                source.Cancel();
            }),
            Task.Run(() =>
            {
                RunMethods(source.Token);
            })
        );
    }
    public Task<TResult?> InvokeAsync<TResult>(string method, params object[] @params)
    {
        return InvokeAsync<TResult>(CancellationToken.None, method, @params);
    }
    public Task<TResult?> InvokeAsync<TResult>(CancellationToken cancellationToken, string method, params object[] @params)
    {
        return Task.Run(() =>
        {
            var id = Guid.NewGuid().ToString();

            var rpccall = new { jsonrpc = "2.0", method, @params, id };

            _dotnetBuffer.WriteLine(JsonConvert.SerializeObject(rpccall));

            while(!cancellationToken.IsCancellationRequested)
            {
                if (_results.TryRemove(id, out var jobj))
                {
                    var error = jobj["error"];
                    if (error != null)
                    {
                        throw new IpcException(
                            ((int?)error["code"]) ?? -1, 
                            ((string?)error["message"]) ?? "Unknown", 
                             error["data"]);
                    }
                    var result = jobj["result"];
                    if (result != null)
                        return result.ToObject<TResult>();

                    throw new NotSupportedException();
                }
            }
            throw new TaskCanceledException();
        }, cancellationToken);
    }
    public async ValueTask DisposeAsync ()
    {
        _dotnetBuffer.WriteLine(CMD_QUIT);
        if (_task != null)
            await _task.ConfigureAwait(false);
        await _dotnetBuffer.DisposeAsync();
    }
    private WasiConfiguration CreateConfig(QjsIpcOptions options)
    {
        options.Validate();

        var config = new WasiConfiguration();

        config = config.WithPreopenedDirectory(options.AllowedDirectoryPath!, ".");
        
        if (!options.DisallowStdIn)
        {
            config =  string.IsNullOrEmpty(options.StdInFilePath) ? config.WithInheritedStandardInput() : config.WithStandardInput(options.StdInFilePath);
        }

        if (!options.DisallowStdOut)
        {
            config =  string.IsNullOrEmpty(options.StdOutFilePath) ? config.WithInheritedStandardOutput() : config.WithStandardOutput(options.StdOutFilePath);
        }
        
        if (!options.DisallowStdErr)
        {
            config =  string.IsNullOrEmpty(options.StdErrFilePath) ? config.WithInheritedStandardError() : config.WithStandardError(options.StdErrFilePath);
        }

        return config.WithArgs("--", options.ScriptFileName!);
    }
    private void RunWasm(QjsIpcOptions options) 
    {
        using var engine = new Engine();
        using var module = Module.FromStream(engine, "qjs", GetType().Assembly.GetManifestResourceStream("QjsIpc.qjs.wasm")!);
        using var linker = new Linker(engine);
        using var store = new Store(engine);
        using var wasmBuffer = new MemoryStream();
        
        linker.DefineFunction("env", "write", (int c) => {
            if (c == '\n')
            {
                var len = wasmBuffer.Position;

                if (len > int.MaxValue)
                    throw new NotSupportedException("The line is too long. It needs to be less than or equal to int.MaxValue.");

                var str = Encoding.UTF8.GetString(wasmBuffer.GetBuffer(), 0, (int)len);

                try {
                    var jobj = JObject.Parse(str);
                    if (jobj.ContainsKey("result") || jobj.ContainsKey("error"))
                    {
                        _results.TryAdd((string)jobj["id"]!, jobj);
                    }
                    else if (jobj.ContainsKey("method"))
                    {
                        _invokes.Enqueue(jobj);
                    }
                }
                catch(JsonException ex){
                    _dotnetBuffer.WriteLine(JsonConvert.SerializeObject(new {
                        jsonrpc = "2.0",
                        id = (string?)null,
                        error = new { 
                            code = -32700, 
                            message = $"Parse error: {ex.Message}" 
                        }
                    }));
                }

                wasmBuffer.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                wasmBuffer.WriteByte((byte)c);
            }
        });

        linker.DefineFunction("env", "read", () => _dotnetBuffer.ReadByte());

        linker.DefineWasi();

        store.SetWasiConfiguration(CreateConfig(options));

        var instance = linker.Instantiate(store, module);

        var run = instance.GetFunction(store, "_start");

        run!.Invoke(store);
    }
    private void RunMethods(CancellationToken cancellationToken)
    {
        if (_methods == null)
            return;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_invokes.TryDequeue(out var jobj))
            {
                _methods.InvokeMethod(jobj, cancellationToken).ContinueWith(task => {
                    _dotnetBuffer.WriteLine(JsonConvert.SerializeObject(task.Result));
                }, cancellationToken);
            }
        }
    }
}
