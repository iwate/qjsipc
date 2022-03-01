using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QjsIpc;

namespace QjsIpc.Tests;

[TestClass]
public class OptionTests
{
    [TestMethod]
    public void EmptyOptionExceptArgNullExt()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new QjsIpcOptions().Validate());
    }

    [TestMethod]
    public void IllegalAllowedDirectoryPathExceptArgExt()
    {
        Assert.ThrowsException<ArgumentException>(() => new QjsIpcOptions
        {
            AllowedDirectoryPath = "/the/illegal/path"
        }.Validate());
    }

    [TestMethod]
    public void IllegalScriptFileNameExceptArgExt()
    {
        Assert.ThrowsException<ArgumentException>(() => new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "illegal"
        }.Validate());
    }

    [TestMethod]
    public void ExistedAllowedDirectoryPathAndScriptFileNameDoesNotExcept()
    {
        new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll"
        }.Validate();
    }

    [TestMethod]
    public void IllegalStdInFilePathExceptArgExt()
    {
        Assert.ThrowsException<ArgumentException>(() => new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll",
            StdInFilePath = "/the/illegal/path"
        }.Validate());
    }

    [TestMethod]
    public void IllegalStdOutFilePathExceptArgExt()
    {
        Assert.ThrowsException<ArgumentException>(() => new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll",
            StdOutFilePath = "/the/illegal/path"
        }.Validate());
    }

    [TestMethod]
    public void IllegalStdErrFilePathExceptArgExt()
    {
        Assert.ThrowsException<ArgumentException>(() => new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll",
            StdErrFilePath = "/the/illegal/path"
        }.Validate());
    }

    [TestMethod]
    public void ExistedStdInFilePathDoesNotExcept()
    {
        new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll",
            StdInFilePath = "./stdin.txt"
        }.Validate();
    }

    [TestMethod]
    public void ExistedStdOutFilePathDoesNotExcept()
    {
        new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll",
            StdOutFilePath = "./stdout.txt"
        }.Validate();
    }

    [TestMethod]
    public void ExistedStdErrFilePathDoesNotExcept()
    {
        new QjsIpcOptions
        {
            AllowedDirectoryPath = AppDomain.CurrentDomain.BaseDirectory,
            ScriptFileName = "QjsIpc.dll",
            StdErrFilePath = "./stderr.txt"
        }.Validate();
    }
}