
using System.Text.Json.Serialization;
using ProjectLogging.Data;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Website;



public class ProjectCard : IModel
{
    public string ProjectTitle { get; set; }
    public string? RemoteUrl { get; set; }
    public string ShortDescription { get; set; }
    public string ReadmePath { get; set; }



    public ProjectCard(string projectTitle, string? remoteUrl, string shortDescription, string readmePath)
    {
        ProjectTitle = projectTitle;
        RemoteUrl = remoteUrl;
        ShortDescription = shortDescription;
        ReadmePath = readmePath;
    }



    public ProjectCard(ProjectReadme projectReadme)
    {
        ProjectTitle = projectReadme.Title;
        RemoteUrl = projectReadme.RemoteUrl;
        ShortDescription = projectReadme.ShortDescription;
        ReadmePath = projectReadme.ReadmePath;
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
