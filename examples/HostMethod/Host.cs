namespace HostMethod;
public class Host
{
    private readonly IDictionary<string, string> _data = new Dictionary<string, string>
    {
        ["Key1"] = "Hello,",
        ["Key2"] = "World!",
        ["Key3"] = "QjsIpc",
    };
    public string? GetValue(string key)
    {
        return _data.ContainsKey(key) ? _data[key] : null;
    }
    public Task<string?> GetValueTask(string key)
    {
        return Task.FromResult(GetValue(key));
    }
    public async Task<string?> GetValueAsync(string key)
    {
        await Task.Delay(0);
        return GetValue(key);
    }
}

