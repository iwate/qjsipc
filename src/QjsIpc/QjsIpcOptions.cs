using System.Text;

namespace QjsIpc;
public class QjsIpcOptions
{
    public string? AllowedDirectoryPath { get; set; }
    public string? ScriptFileName { get; set; }
    public string? StdInFilePath { get; set; }
    public string? StdOutFilePath { get; set; }
    public string? StdErrFilePath { get; set; }
    public bool DisallowStdIn { get; }
    public bool DisallowStdOut { get; }
    public bool DisallowStdErr { get; }
    public object? MethodsHost { get; set; }

    internal void Validate()
    {
        if (string.IsNullOrEmpty(AllowedDirectoryPath))
            throw new ArgumentNullException(nameof(AllowedDirectoryPath));

        else if (!Directory.Exists(AllowedDirectoryPath))
            throw new ArgumentException($"'{AllowedDirectoryPath}' does not exist.");

        if (string.IsNullOrEmpty(ScriptFileName))
            throw new ArgumentNullException(nameof(ScriptFileName));

        else if (!File.Exists(Path.Combine(AllowedDirectoryPath, ScriptFileName)))
            throw new ArgumentException($"'{Path.Combine(AllowedDirectoryPath, ScriptFileName)}' does not exist.");

        ValidateEncoding();

        if (!string.IsNullOrEmpty(StdInFilePath)) {
            var stdinDir = new FileInfo(StdInFilePath).Directory;
            if (stdinDir == null)
                throw new ArgumentException(nameof(stdinDir));

            if (!stdinDir.Exists)
                throw new ArgumentException($"'{stdinDir.FullName}' does not exist.");
        }

        if (!string.IsNullOrEmpty(StdOutFilePath)) {
            var stdoutDir = new FileInfo(StdOutFilePath).Directory;
            if (stdoutDir == null)
                throw new ArgumentException(nameof(stdoutDir));

            if (!stdoutDir.Exists)
                throw new ArgumentException($"'{stdoutDir.FullName}' does not exist.");
        }

        if (!string.IsNullOrEmpty(StdErrFilePath)) {
            var stderrDir = new FileInfo(StdErrFilePath).Directory;
            if (stderrDir == null)
                throw new ArgumentException(nameof(stderrDir));

            if (!stderrDir.Exists)
                throw new ArgumentException($"'{stderrDir.FullName}' does not exist.");
        }
    }
    internal void ValidateEncoding() 
    {
        var buffer = new byte[5];
        int length;
        using (var file = new FileStream(Path.Combine(AllowedDirectoryPath!, ScriptFileName!), FileMode.Open))
        {
            length = file.Read(buffer, 0, 5);
        }

        if (length >= 3 && buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
            throw new ArgumentException("The script has BOM code. It's need UTF8 without BOM.");

        if (length >= 4 && buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0xfe && buffer[3] == 0xff)
            throw new ArgumentException("The script might be UTF32 BE. It's need UTF8 without BOM.");

        if (length >= 4 && buffer[0] == 0xff && buffer[1] == 0xfe && buffer[2] == 0x00 && buffer[3] == 0x00)
            throw new ArgumentException("The script might be UTF32 LE. It's need UTF8 without BOM.");

        if (length >= 2 && buffer[0] == 0xfe && buffer[1] == 0xff)
            throw new ArgumentException("The script might be UTF16 BE. It's need UTF8 without BOM.");

        if (length >= 2 && buffer[0] == 0xff && buffer[1] == 0xfe)
            throw new ArgumentException("The script might be UTF16 LE. It's need UTF8 without BOM.");
    }
}