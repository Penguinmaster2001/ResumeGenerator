
using System.Text.Json.Serialization;



namespace ProjectLogging.WebsiteGeneration;



public class TemplateSettings
{
    public Dictionary<string, string> TemplateNames { get; }
    public Dictionary<string, string> TemplateUses { get; }



    [JsonConstructor]
    public TemplateSettings(Dictionary<string, string> templateNames, Dictionary<string, string> templateUses)
    {
        TemplateNames = templateNames;
        TemplateUses = templateUses;
    }
}
