
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class HtmlTemplate : IHtmlItem
{
    private readonly string _template;
    public object? Data { get; set; } = null;
    public bool Strict { get; set; } = true;



    public HtmlTemplate(string template, object? data = null)
    {
        _template = template;
        Data = data;
    }



    public static async Task<HtmlTemplate> LoadFromFile(string path, object? data = null)
    {
        using var file = new StreamReader(path);

        return new(await file.ReadToEndAsync(), data);
    }



    public string GenerateHtml()
    {
        if (Data is null)
        {
            if (Strict)
            {
                throw new Exception("Strict is enabled but Data is null.");
            }

            return _template;
        }

        return GenerateHtml(Data);
    }



    public string GenerateHtml(object data)
    {
        throw new NotImplementedException();
    }
}
