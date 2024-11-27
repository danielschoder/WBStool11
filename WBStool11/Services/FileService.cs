using System.IO;
using System.Text.Json;

namespace WBStool11.Services;

public interface IFileService
{
    void SaveToFile<T>(string filePath, T data);
    T ReadFromFile<T>(string filePath);
}

public class FileService : IFileService
{
    public void SaveToFile<T>(string filePath, T data)
    {
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, json);
    }

    public T ReadFromFile<T>(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}
