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
}