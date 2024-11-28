using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WBStool11.Services;

public interface IFileService
{
    Task SaveToFileAsync<T>(string filePath, T data);
    Task<T> ReadFromFileAsync<T>(string filePath);
}

public class FileService : IFileService
{
    private readonly JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task SaveToFileAsync<T>(string filePath, T data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        await File.WriteAllTextAsync(filePath, json, Encoding.Default);
    }

    public async Task<T> ReadFromFileAsync<T>(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath, Encoding.Default);
        return JsonSerializer.Deserialize<T>(json);
    }
}
