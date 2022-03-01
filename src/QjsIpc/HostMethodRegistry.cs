using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QjsIpc;

internal class HostMethodRegistry
{
    private IReadOnlyDictionary<string, MethodInfo> _methods;
    private object _methodHost;

    private HostMethodRegistry(object host, IReadOnlyDictionary<string, MethodInfo> methods)
    {
        _methodHost = host;
        _methods = methods;
    }

    public static HostMethodRegistry Create(object host)
    {
        return new HostMethodRegistry(host, host.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).ToDictionary(m => m.Name, m => m));
    }

    private static readonly Type _valueTaskType = typeof(ValueTask<>);
    private static readonly Type _taskType = typeof(Task<>);
    public Task<object> InvokeMethod(JObject jobj, CancellationToken cancellationToken)
    {
        var jsonrpc = "2.0";
        var id = (string?)jobj["id"];
        var methodName = (string?)jobj["method"];
        var @params = jobj["params"];
        if (string.IsNullOrEmpty(methodName))
        {
            return Task.FromResult((object)new {
                jsonrpc, id,
                error = new { 
                    code = -32600, 
                    message = "Invalid Request: 'method' property is not found." 
                }
            });
        }
        if (@params == null)
        {
            return Task.FromResult((object)new {
                jsonrpc, id,
                error = new { 
                    code = -32600, 
                    message = "Invalid Request: 'params' property is not found." 
                }
            });
        }
        if (!_methods.ContainsKey(methodName))
        {
            return Task.FromResult((object)new {
                jsonrpc, id,
                error = new { 
                    code = -32601, 
                    message = $"Method not found: '{methodName}' is not found." 
                }
            });
        }

        var method = _methods[methodName];
        var types = method.GetParameters();
        var args = new List<object?>(types.Length);
        try {
            foreach (var (token, info) in @params.Zip(types))
            {
                args.Add(token.ToObject(info.ParameterType));
            }
        }
        catch (JsonException ex)
        {
            return Task.FromResult((object)new {
                jsonrpc, id,
                error = new { 
                    code = -32602, 
                    message = $"Invalid params: {ex.Message}" 
                }
            });
        }
        try
        {
            var result = method.Invoke(_methodHost, args.ToArray());
            if (result == null) 
            {
                return Task.FromResult((object)new 
                {
                    jsonrpc, id,
                    result = (object?)null
                });
            }
            if (method.ReturnType.IsGenericType) {
                var typeDef = method.ReturnType.GetGenericTypeDefinition();
                var resultTask = typeDef == _taskType ? (Task?)result
                               : typeDef == _valueTaskType ? (Task?)(result.GetType().GetMethod("AsTask")?.Invoke(result, null))
                               : null;
                if (resultTask != null) 
                {
                    return resultTask.ContinueWith(task =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            return (object)new
                            {
                                jsonrpc, id,
                                result = task.GetType().GetProperty("Result")?.GetValue(resultTask)
                            };
                        }
                        else
                        {
                            return (object)new
                            {
                                jsonrpc, id,
                                error = new { 
                                    code = -32603,
                                    message = $"Internal error: {task.Exception?.Message}"
                                }
                            };
                        }
                    }, cancellationToken);
                }
            }
            
            return Task.FromResult((object)new
            {
                jsonrpc, id,
                result
            });
        }
        catch(Exception ex)
        {
            return Task.FromResult((object)new {
                jsonrpc, id,
                error = new { 
                    code = -32603, 
                    message = $"Internal error: {ex.Message}" 
                }
            });
        }
    }
}