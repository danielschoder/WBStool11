using System.IO;
using System.Text;
using System.Text.Json;

namespace WBStool11.Services;

public interface IFileService
{
    Task SaveToFileAsync<T>(string filePath, T data);
    Task<T> ReadFromFileAsync<T>(string filePath);
}

public class FileService : IFileService
{
    public async Task SaveToFileAsync<T>(string filePath, T data)
    {
        var json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, json, Encoding.Default);
    }

    public async Task<T> ReadFromFileAsync<T>(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}
