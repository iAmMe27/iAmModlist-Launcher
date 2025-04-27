using System.IO;
using System.Text.Json;
using iAmModlist_Launcher.Launcher.Settings;

namespace iAmModlist_Launcher.Launcher.Json;

public static class JsonInterface
{
    public static async Task<T?> Read<T>(string filename)
    {
        await using FileStream openStream = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        return await JsonSerializer.DeserializeAsync<T>(openStream);
    }

    public static async Task Write<T>(string filename, T input)
    {
        await using FileStream createStream = new(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
        await JsonSerializer.SerializeAsync(createStream, input);
    }
}