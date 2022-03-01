using System.Collections.Generic;
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
}