using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace QjsIpc;

/// <summary>
/// Write many read one buffer
/// </summary>
internal class ConcurrentWritableBuffer : IAsyncDisposable
{
    private readonly ConcurrentQueue<MemoryStream> _usings = new ConcurrentQueue<MemoryStream>();
    private readonly ConcurrentQueue<MemoryStream> _empties = new ConcurrentQueue<MemoryStream>();

    private MemoryStream NewWriteSream()
    {
        if (_empties.Count > 0 && _empties.TryDequeue(out var stream))
            return stream;
        
        return new MemoryStream();
    }

    private void FinishWriting(MemoryStream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        _usings.Enqueue(stream);
    }

    private MemoryStream? NewReadStream()
    {
        while (_usings.Count > 0)
        {
            if (_usings.TryDequeue(out var stream))
                return stream;
        }

        return null;
    }

    private void FinishReading(MemoryStream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        stream.SetLength(0);
        _empties.Enqueue(stream);
    }

    public void WriteLine(string line)
    {
        var bytes = Encoding.UTF8.GetBytes(line);
        var stream = NewWriteSream();
        stream.Write(bytes);
        stream.WriteByte((byte)'\n');
        FinishWriting(stream);
    }
    
    private MemoryStream? _currentReadStream = null;
    public int ReadByte()
    {
        if (_currentReadStream == null)
        {
            while ((_currentReadStream = NewReadStream()) == null);
        }
        else if (_currentReadStream.Position >= _currentReadStream.Length)
        {
            FinishReading(_currentReadStream);
            while ((_currentReadStream = NewReadStream()) == null);
        }

        return _currentReadStream.ReadByte();
    }

    public async ValueTask DisposeAsync()
    {
        while (_usings.Count > 0)
        {
            if (_usings.TryDequeue(out var stream)) 
                await stream.DisposeAsync();
        }
        
        while (_empties.Count > 0)
        {
            if (_empties.TryDequeue(out var stream))
                await stream.DisposeAsync();
        }
    }
}
