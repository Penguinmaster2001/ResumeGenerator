
using System.Text.Json;
using ProjectLogging.Data;



namespace ProjectLogging.Projects;



public static class RecordLoader
{
    public async static Task<List<T>> LoadRecordsAsync<T>(string filePath) where T : BaseModel
        => await LoadRecordsAsync<T>(File.OpenRead(filePath));

    public async static Task<List<T>> LoadRecordsAsync<T>(Stream stream) where T : BaseModel
        => await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new();


    public async static Task<PersonalInfo> LoadPersonalInfoAsync(string filePath)
        => await LoadPersonalInfoAsync(File.OpenRead(filePath));

    public async static Task<PersonalInfo> LoadPersonalInfoAsync(Stream stream)
        => await JsonSerializer.DeserializeAsync<PersonalInfo>(stream)
            ?? new(string.Empty, string.Empty, string.Empty, string.Empty, new());
}
