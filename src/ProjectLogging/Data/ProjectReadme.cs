
using System.Text.Json;

namespace ProjectLogging.Data;



public record ProjectReadme(
    string Title,
    string RepoPath,
    string ReadmePath,
    DateOnly? StartDate,
    DateOnly EndDate,
    string ShortDescription)
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };
}
