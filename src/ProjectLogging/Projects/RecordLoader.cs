
using System.Text.Json;
using ProjectLogging.Data;



namespace ProjectLogging.Projects;



public static class RecordLoader
{
    public async static Task<List<T>> LoadRecordsAsync<T>(string filePath) where T : BaseData
    {
        try
        {
            return await LoadRecordsAsync<T>(File.OpenRead(filePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine(filePath);
            Console.WriteLine(ex);
            throw;
        }
    }

    public async static Task<List<T>> LoadRecordsAsync<T>(Stream stream) where T : BaseData
        => await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? [];


    public async static Task<PersonalInfo> LoadPersonalInfoAsync(string filePath)
        => await LoadPersonalInfoAsync(File.OpenRead(filePath));

    public async static Task<PersonalInfo> LoadPersonalInfoAsync(Stream stream)
        => await JsonSerializer.DeserializeAsync<PersonalInfo>(stream)
            ?? new(string.Empty, string.Empty, string.Empty, string.Empty, []);

    public async static Task<List<ProjectReadme>> LoadProjectReadmeAsync(string filePath)
    {
        var readmes = await JsonSerializer.DeserializeAsync<List<ProjectReadme>>(File.OpenRead(filePath), ProjectReadme.JsonOptions);

        return readmes ?? [];
    }
}
