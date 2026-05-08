
using System.Text;
using System.Text.RegularExpressions;
using ProjectLogging.Views;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public partial class HtmlTemplate : IHtmlItem
{
    [GeneratedRegex("""{{ ?(?<variable>(\.?\w+)+)( ?\|(?<formats>( ?\w+)+))? ?}}""")]
    private static partial Regex _templateVariableRegex { get; }

    [GeneratedRegex("""{% ?for (?<itemLabel>\w+) in (?<collectionName>\w+) ?%}(?<content>.*?){% ?endfor ?%}""", RegexOptions.Singleline)]
    private static partial Regex _templateEnumerableRegex { get; }

    private static readonly Dictionary<string, Func<string, string>> _formatters = new()
    {
        ["upper"] = s => s.ToUpper(),
        ["lower"] = s => s.ToLower(),
        ["snake"] = s => s.SnakeCase(),
    };



    private readonly string _template;
    public object? Data { get; set; } = null;
    public bool Strict { get; set; } = false;



    public HtmlTemplate(string template, object? data = null)
    {
        _template = template;
        Data = data;
    }



    public static async Task<HtmlTemplate> LoadFromFileAsync(string path, object? data = null)
    {
        using var file = new StreamReader(path);

        return new(await file.ReadToEndAsync(), data);
    }



    public string GenerateHtml()
    {
        if (Data is null) return _template;

        return GenerateHtml(Data);
    }



    public string GenerateHtml(object data)
    {
        var dataCache = IDataCache.Create(data);

        var enumerableReplacement = _templateEnumerableRegex.Replace(_template, (m) =>
        {
            if (m.Groups.TryGetValue("itemLabel", out var nameGroup)
                && m.Groups.TryGetValue("collectionName", out var collectionNameGroup)
                && m.Groups.TryGetValue("content", out var contentGroup)
                && dataCache.TryGetValue(collectionNameGroup.Value, out var value))
            {
                var enumerableTemplate = ReplaceVariables(contentGroup.Value, dataCache, false);

                if (value is not IEnumerable<object> enumerable)
                {
                    if (Strict)
                    {
                        throw new Exception($"\"{collectionNameGroup.Value}\" is not enumerable.");
                    }

                    return m.Value;
                }

                var sb = new StringBuilder();

                foreach (var item in enumerable)
                {
                    sb.Append(ReplaceVariables(enumerableTemplate, dataCache.Extended(nameGroup.Value, item)));
                }

                return sb.ToString();
            }

            if (Strict) throw new Exception("");

            return m.Value;
        });

        return ReplaceVariables(enumerableReplacement, dataCache);
    }



    private string ReplaceVariables(string template, IDataCache dataCache, bool? overrideStrict = null)
    {
        return _templateVariableRegex.Replace(template, (m) =>
        {
            if (m.Groups.TryGetValue("variable", out var variableGroup)
                && dataCache.TryGetValue(variableGroup.Value, out var value))
            {
                var variableString = value.ToString() ?? ((overrideStrict ?? Strict)
                    ? throw new Exception($"Unknown variable \"{variableGroup.Value}\". Available variables: {string.Join(", ", dataCache.VariableNames)}")
                    : m.Value);

                return m.Groups.TryGetValue("formats", out var formats)
                    ? FormatVariable(variableString, formats.Value, false)
                    : variableString;
            }

            if (overrideStrict ?? Strict) throw new Exception($"Variable not found \"{variableGroup?.Value ?? m.Value}\". Available variables: {string.Join(", ", dataCache.VariableNames)}");

            return m.Value;
        });
    }



    private string FormatVariable(string value, string formats, bool? overrideStrict = null)
    {
        foreach (var format in formats.Split(' '))
        {
            if (_formatters.TryGetValue(format.ToLower(), out var formatter))
            {
                value = formatter(value);
            }
            else if (overrideStrict ?? Strict)
            {
                throw new Exception($"Invalid format \"{format}\".");
            }
        }

        return value;
    }
}
