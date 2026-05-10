
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public class TemplateManager(Dictionary<string, string> templates) : ITemplateManager
{
    private readonly Dictionary<string, string> _templates = templates;
    private IDataCache _baseData = IDataCache.Create();



    public TemplateManager() : this([]) { }



    public void AddTemplate(string name, string template) => _templates.Add(name, template);



    public void AddBaseData(object data) => _baseData = _baseData.Combined(IDataCache.Create(data));
    public void AddBaseData(string name, object data) => _baseData = _baseData.Extended(name, data);



    public HtmlTemplate Create(string name, object? data)
        => new(_templates[name], _baseData.Combined(IDataCache.Create(data)));
}
