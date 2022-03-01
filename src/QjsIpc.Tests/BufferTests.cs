using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QjsIpc;

namespace QjsIpc.Tests;  


[TestClass]
public class BufferTests
{
    [TestMethod]
    public void WriteLineBufferTest()
    {
        var buffer = new ConcurrentWritableBuffer();

        buffer.WriteLine("Test");
        Assert.AreEqual('T', buffer.ReadByte());
        Assert.AreEqual('e', buffer.ReadByte());
        Assert.AreEqual('s', buffer.ReadByte());
        Assert.AreEqual('t', buffer.ReadByte());
        Assert.AreEqual('\n', buffer.ReadByte());
    }
}