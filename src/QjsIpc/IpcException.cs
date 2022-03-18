using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace QjsIpc;
public class IpcException : Exception
{
    public int Code { get; set; }
    public IpcException(int code, string message, IDictionary<string, object>? data)
        : base(message)
    {
        Code = code;
        if (data != null){
            foreach(var kv in data) {
                Data.Add(kv.Key, kv.Value);
            }
        }
    }

    public IpcException(int code, string message, JToken? token)
        : this(code, message, ExtractData(token))
    {
    }

    static IDictionary<string, object>? ExtractData(JToken? token)
    {
        if (token == null || token.Type == JTokenType.None || token.Type == JTokenType.Null || token.Type == JTokenType.Null)
            return null;

        if (token.Type == JTokenType.Object)
            return token.ToObject<Dictionary<string, object>>();

        return new Dictionary<string, object>
        {
            ["data"] = token
        };
    }
}