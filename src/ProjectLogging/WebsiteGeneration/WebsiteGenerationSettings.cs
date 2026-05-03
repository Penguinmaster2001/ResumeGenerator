
using System.Text.Json.Serialization;



namespace ProjectLogging.WebsiteGeneration;



public class WebsiteGenerationSettings
{
    public string BasePath { get; }
    public string StylesPath { get; }
    public string Style { get; }
    public string WebsiteOutputPath { get; }
    public string DataConfigPath { get; }
    public string TemplateSettingsPath { get; }



    [JsonConstructor]
    public WebsiteGenerationSettings(
        string basePath,
        string stylesPath,
        string style,
        string websiteOutputPath,
        string dataConfigPath,
        string templateSettingsPath)
    {
        BasePath = Path.GetFullPath(basePath);
        StylesPath = GetFullPath(stylesPath);
        Style = style;
        WebsiteOutputPath = GetFullPath(websiteOutputPath);
        DataConfigPath = GetFullPath(dataConfigPath);
        TemplateSettingsPath = GetFullPath(templateSettingsPath);
    }



    public string GetFullPath(string path)
    {
        return Path.GetFullPath(path, BasePath);
    }
}
