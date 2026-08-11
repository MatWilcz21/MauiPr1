using System.Text.Json;

namespace MauiApp1;

static class JsonHandler
{


    public static async Task SaveJson(object objectToSave, string fileName)
    {

        string path = Path.Combine(FileSystem.Current.AppDataDirectory, $"{fileName}.json");

        string json = JsonSerializer.Serialize(objectToSave);

        await File.WriteAllTextAsync(path, json);
    }

    public static async Task<T?> LoadJson<T>(string fileName)
    {
        string path = Path.Combine(FileSystem.Current.AppDataDirectory, $"{fileName}.json");

        if (!File.Exists(path))
            return default;

        string json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });


    }
}
