
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public class TemplateManager(Dictionary<string, string> templates) : ITemplateManager
{
    private readonly Dictionary<string, string> _templates = templates;



    public TemplateManager() : this([]) { }



    public void AddTemplate(string name, string template) => _templates.Add(name, template);



    public HtmlTemplate Create(string name, object data) => new(_templates[name], data);
}
