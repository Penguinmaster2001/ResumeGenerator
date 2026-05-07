
using System.Text.Json.Serialization;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteGenerationSettings
{
    public string BasePath { get; }
    public string StylesPath { get; }
    public Dictionary<string, string> Styles { get; }
    public string TemplatesPath { get; }
    public string WebsiteOutputPath { get; }
    public string DataConfigPath { get; }
    public string TemplateSettingsPath { get; }



    [JsonConstructor]
    public WebsiteGenerationSettings(
        string basePath,
        string stylesPath,
        Dictionary<string, string> styles,
        string templatesPath,
        string websiteOutputPath,
        string dataConfigPath,
        string templateSettingsPath)
    {
        BasePath = Path.GetFullPath(basePath);
        StylesPath = GetFullPath(stylesPath);
        TemplatesPath = GetFullPath(templatesPath);
        Styles = styles;
        WebsiteOutputPath = GetFullPath(websiteOutputPath);
        DataConfigPath = GetFullPath(dataConfigPath);
        TemplateSettingsPath = GetFullPath(templateSettingsPath);
    }



    public string GetFullPath(string path)
    {
        return Path.GetFullPath(path, BasePath);
    }
}
