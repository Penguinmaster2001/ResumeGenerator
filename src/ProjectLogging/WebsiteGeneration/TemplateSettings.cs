
using System.Text.Json.Serialization;



namespace ProjectLogging.WebsiteGeneration;



public class TemplateSettings
{
    public Dictionary<string, string> TemplateNames { get; }



    [JsonConstructor]
    public TemplateSettings(Dictionary<string, string> templateNames)
    {
        TemplateNames = templateNames;
    }
}
