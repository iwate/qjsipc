using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QjsIpc;

namespace QjsIpc.Tests;  


[TestClass]
public class HostMethodTests
{
    [TestMethod]
    public async Task ReturnValuesAreSucceeded()
    {
        var methods = HostMethodRegistry.Create(new TestHost());
        
        dynamic result1 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual("Hello", result1.result);

        dynamic result2 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.IntMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(100, result2.result);

        dynamic result3 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.DoubleMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(0.02d, result3.result);

        dynamic result4 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result4.result, typeof(TestObject));

        dynamic result5 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringArrayMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result5.result, typeof(string[]));

        dynamic result6 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectArrayMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result6.result, typeof(TestObject[]));
    }

    [TestMethod]
    public async Task ReturnTasksAreSucceeded()
    {
        var methods = HostMethodRegistry.Create(new TestHost());
        
        dynamic result1 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringTaskMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual("Hello", result1.result);

        dynamic result2 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.IntTaskMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(100, result2.result);

        dynamic result3 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.DoubleTaskMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(0.02d, result3.result);

        dynamic result4 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectTaskMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result4.result, typeof(TestObject));

        dynamic result5 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringArrayTaskMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result5.result, typeof(string[]));

        dynamic result6 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectArrayTaskMethod),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result6.result, typeof(TestObject[]));
    }

    [TestMethod]
    public async Task ReturnTaskAsyncsAreSucceeded()
    {
        var methods = HostMethodRegistry.Create(new TestHost());
        
        dynamic result1 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual("Hello", result1.result);

        dynamic result2 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.IntTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(100, result2.result);

        dynamic result3 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.DoubleTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(0.02d, result3.result);

        dynamic result4 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result4.result, typeof(TestObject));

        dynamic result5 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringArrayTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result5.result, typeof(string[]));

        dynamic result6 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectArrayTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result6.result, typeof(TestObject[]));
    }

    [TestMethod]
    public async Task ReturnValueTaskAsyncsAreSucceeded()
    {
        var methods = HostMethodRegistry.Create(new TestHost());
        
        dynamic result1 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringValueTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual("Hello", result1.result);

        dynamic result2 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.IntValueTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(100, result2.result);

        dynamic result3 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.DoubleValueTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.AreEqual(0.02d, result3.result);

        dynamic result4 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectValueTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result4.result, typeof(TestObject));

        dynamic result5 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.StringArrayValueTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result5.result, typeof(string[]));

        dynamic result6 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ObjectArrayValueTaskMethodAsync),
            @params = new object[0]
        }), CancellationToken.None);

        Assert.IsInstanceOfType(result6.result, typeof(TestObject[]));
    }

    [TestMethod]
    public async Task ParamsTestIsSucceeded()
    {
        var methods = HostMethodRegistry.Create(new TestHost());
        
        dynamic result1 = await methods.InvokeMethod(JObject.FromObject(new {
            id = "0",
            method = nameof(TestHost.ParamsMethod),
            @params = new object[]
            {
                (object)"Hello",
                (object)100,
                (object)0.02d,
                (object)new TestObject
                {
                    String = "World",
                    Int = 100,
                    Double = 0.02d,
                }
            }
        }), CancellationToken.None);
        
        Assert.IsInstanceOfType(result1.result, typeof(TestObject));
        Assert.AreEqual("Hello", result1.result.String);
        Assert.AreEqual(100, result1.result.Int);
        Assert.AreEqual(0.02d, result1.result.Double);
        Assert.IsNotNull(result1.result.Object);
        Assert.AreEqual("World", result1.result.Object.String);
        Assert.AreEqual(100, result1.result.Object.Int);
        Assert.AreEqual(0.02d, result1.result.Object.Double);
    }
}

public class TestHost 
{
    public string StringMethod() => "Hello";
    public int IntMethod() => 100;
    public double DoubleMethod() => 0.02;
    public TestObject ObjectMethod() => new TestObject();
    public string[] StringArrayMethod() => new [] { "Hello", "World" };
    public TestObject[] ObjectArrayMethod() => new [] { new TestObject(), new TestObject() };

    public Task<string> StringTaskMethod() => Task.FromResult("Hello");
    public Task<int> IntTaskMethod() =>  Task.FromResult(100);
    public Task<double> DoubleTaskMethod() =>  Task.FromResult(0.02d);
    public Task<TestObject> ObjectTaskMethod() =>  Task.FromResult(new TestObject());
    public Task<string[]> StringArrayTaskMethod() =>  Task.FromResult(new [] { "Hello", "World" });
    public Task<TestObject[]> ObjectArrayTaskMethod() =>  Task.FromResult(new [] { new TestObject(), new TestObject() });

    public async Task<string> StringTaskMethodAsync() 
    {
        await Task.Delay(0);
        return "Hello";
    }
    public async Task<int> IntTaskMethodAsync()
    {
        await Task.Delay(0);
        return 100;
    }
    public async Task<double> DoubleTaskMethodAsync()
    {
        await Task.Delay(0);
        return 0.02d;
    }
    public async Task<TestObject> ObjectTaskMethodAsync()
    {
        await Task.Delay(0);
        return new TestObject();
    }
    public async Task<string[]> StringArrayTaskMethodAsync()
    {
        await Task.Delay(0);
        return new [] { "Hello", "World" };
    }
    public async Task<TestObject[]> ObjectArrayTaskMethodAsync()
    {
        await Task.Delay(0);
        return new [] { new TestObject(), new TestObject() };
    }

    public async ValueTask<string> StringValueTaskMethodAsync() 
    {
        await Task.Delay(0);
        return "Hello";
    }
    public async ValueTask<int> IntValueTaskMethodAsync()
    {
        await Task.Delay(0);
        return 100;
    }
    public async ValueTask<double> DoubleValueTaskMethodAsync()
    {
        await Task.Delay(0);
        return 0.02d;
    }
    public async ValueTask<TestObject> ObjectValueTaskMethodAsync()
    {
        await Task.Delay(0);
        return new TestObject();
    }
    public async ValueTask<string[]> StringArrayValueTaskMethodAsync()
    {
        await Task.Delay(0);
        return new [] { "Hello", "World" };
    }
    public async ValueTask<TestObject[]> ObjectArrayValueTaskMethodAsync()
    {
        await Task.Delay(0);
        return new [] { new TestObject(), new TestObject() };
    }

    public TestObject ParamsMethod(string str, int i, double d, TestObject o)
    {
        return new TestObject 
        {
            String = str,
            Int = i,
            Double = d,
            Object = o,
        };
    }
}

public class TestObject 
{
    public string? String { get; set; }
    public int Int { get; set; }
    public double Double { get; set; }
    public TestObject? Object { get; set; }
}