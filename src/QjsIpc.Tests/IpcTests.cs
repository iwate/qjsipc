using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QjsIpc;

namespace QjsIpc.Tests;  


[TestClass]
public class IpcTests
{
    [TestMethod]
    public async Task EchoTest()
    {
        await using var qjs = new QjsIpcEngine();

        qjs.Start(new QjsIpcOptions
        {
            ScriptFileName = "main.js",
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            MethodsHost = new IpcTestHost()
        });

        var transformed = await qjs.InvokeAsync<JObject>("echo", new { name = "test" });

        Assert.IsNotNull(transformed);
        Assert.AreEqual("test", (string?)transformed["name"]);
    }
    [TestMethod]
    public async Task TransformTest()
    {
        await using var qjs = new QjsIpcEngine();

        qjs.Start(new QjsIpcOptions
        {
            ScriptFileName = "main.js",
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            MethodsHost = new IpcTestHost()
        });

        var transformed = await qjs.InvokeAsync<JObject>("transform", new { name = "test" });

        Assert.IsNotNull(transformed);
        Assert.AreEqual("test", (string?)transformed["name"]);
        Assert.AreEqual("Hello, World", (string?)transformed["message"]);
    }
}

public class IpcTestHost
{
    public string Echo(string message) => message;
}