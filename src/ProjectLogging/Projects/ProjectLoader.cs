
using System.Text.Json;

using ProjectLogging.Records;



namespace ProjectLogging.Projects;



public static class RecordLoader
{
    public async static Task<List<T>> LoadRecordsAsync<T>(Stream stream) where T : IRecord
        => await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new();


    public async static Task<PersonalInfo> LoadPersonalInfoAsync(Stream stream)
        => await JsonSerializer.DeserializeAsync<PersonalInfo>(stream) ?? new(string.Empty, string.Empty, string.Empty, string.Empty, new());
}
