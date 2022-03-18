using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QjsIpc;

namespace QjsIpc.Tests;  
[TestClass]
public class ErrorTests
{
    [TestMethod]
    public async Task RejectObjectTest()
    {
        await using var qjs = new QjsIpcEngine();

        qjs.Start(new QjsIpcOptions
        {
            ScriptFileName = "errors.js",
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory
        });

        await Assert.ThrowsExceptionAsync<IpcException>(async () => {
            await qjs.InvokeAsync<string>("willRejectObject");
        });
    }

    [TestMethod]
    public async Task RejectStringTest()
    {
        await using var qjs = new QjsIpcEngine();

        qjs.Start(new QjsIpcOptions
        {
            ScriptFileName = "errors.js",
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory
        });

        await Assert.ThrowsExceptionAsync<IpcException>(async () => {
            await qjs.InvokeAsync<string>("willRejectString");
        });
    }

    [TestMethod]
    public async Task MissingFuncTest()
    {
        await using var qjs = new QjsIpcEngine();

        qjs.Start(new QjsIpcOptions
        {
            ScriptFileName = "errors.js",
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory
        });

        await Assert.ThrowsExceptionAsync<IpcException>(async () => {
            await qjs.InvokeAsync<string>("missingfunc");
        });
    }
}