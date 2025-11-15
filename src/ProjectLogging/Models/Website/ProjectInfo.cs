
using ProjectLogging.Projects;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Website;



public class ProjectInfo : IModel
{
    public string ProjectTitle { get; }
    public string? ShortDescription { get; }
    public string? Description { get; }
    public string? Motivation { get; }
    public List<string>? Goals { get; }
    public List<string>? BuiltWith { get; }
    public List<string[]>? Features { get; }
    public ProjectReadmeParseResult ReadmeResult { get; }



    public ProjectInfo(
        string projectTitle,
        string? shortDescription,
        string? description = null,
        string? motivation = null,
        List<string>? goals = null,
        List<string>? builtWith = null,
        List<string[]>? features = null,
        ProjectReadmeParseResult? readmeResult = null)
    {
        ProjectTitle = projectTitle;
        ShortDescription = shortDescription;
        Motivation = motivation;
        Goals = goals;
        BuiltWith = builtWith;
        Description = description;
        Features = features;
        ReadmeResult = readmeResult ?? ProjectReadmeParseResult.Failure;
    }



    public static async Task<ProjectInfo> CreateFromCardAsync(ProjectCard card)
    {
        var parseResult = await ProjectReadmeParser.ParseReadmeAsync(card.ReadmePath);

        if (!parseResult.Successful)
        {
            return new(card.ProjectTitle, card.ShortDescription);
        }

        return new(
            card.ProjectTitle,
            card.ShortDescription,
            parseResult.GetSectionContentOrDefault<string>("description", null, StringComparison.OrdinalIgnoreCase),
            parseResult.GetSectionContentOrDefault<string>("motivation", null, StringComparison.OrdinalIgnoreCase),
            parseResult.GetSectionContentOrDefault<List<string>>("goals", null, StringComparison.OrdinalIgnoreCase),
            parseResult.GetSectionContentOrDefault<List<string>>("built with", null, StringComparison.OrdinalIgnoreCase),
            parseResult.GetSectionContentOrDefault<List<string[]>>("features", null, StringComparison.OrdinalIgnoreCase)
        );
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}