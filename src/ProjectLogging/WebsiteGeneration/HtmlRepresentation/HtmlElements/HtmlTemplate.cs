
using System.Text;
using System.Text.RegularExpressions;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public partial class HtmlTemplate : IHtmlItem
{
    [GeneratedRegex("""{{ ?(?<name>\w+)(\.\w+)*( ?\|( ?\w+)+)? ?}}""")]
    private static partial Regex _templateVariableRegex { get; }

    [GeneratedRegex("""{% ?for (?<itemLabel>\w+) in (?<collectionName>\w+) ?%}(?<content>.*?){% ?endfor ?%}""", RegexOptions.Singleline)]
    private static partial Regex _templateEnumerableRegex { get; }


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
        var dataStrings = data
            .GetType()
            .GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(typeof(string)))
            .Select(p => (name: p.Name, value: p.GetValue(data)))
            .Where(p => p.value is not null)
            .ToDictionary(p => p.name, p => p.value!.ToString());

        var dataStringEnumerables = data
            .GetType()
            .GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(typeof(IEnumerable<string>)))
            .Select(p => (name: p.Name, value: p.GetValue(data)))
            .Where(p => p.value is not null)
            .ToDictionary(p => p.name, p => p.value as IEnumerable<string>);

        var enumerableReplacement = _templateEnumerableRegex.Replace(_template, (m) =>
        {
            if (m.Groups.TryGetValue("itemLabel", out var nameGroup)
                && m.Groups.TryGetValue("collectionName", out var collectionNameGroup)
                && m.Groups.TryGetValue("content", out var contentGroup)
                && dataStringEnumerables.TryGetValue(collectionNameGroup.Value, out var enumerable))
            {
                var enumerableTemplate = ReplaceVariables(contentGroup.Value, dataStrings, true);

                var sb = new StringBuilder();

                foreach (var item in enumerable!)
                {
                    sb.Append(ReplaceVariable(nameGroup.Value, item, enumerableTemplate));
                }

                return sb.ToString();
            }

            if (Strict) throw new Exception("");

            return m.Name;
        });

        return ReplaceVariables(enumerableReplacement, dataStrings);
    }



    private string ReplaceVariables(string template, Dictionary<string, string?> variableValues, bool overrideStrict = false)
    {
        return _templateVariableRegex.Replace(template, (m) =>
        {
            if (m.Groups.TryGetValue("name", out var nameGroup)
                && variableValues.TryGetValue(nameGroup.Value, out var value))
            {
                return FormatVariable(value!, m.Groups[3].Captures);;
            }

            if (overrideStrict || Strict) throw new Exception($"Unknown variable {nameGroup!.Name}.");

            return m.Name;
        });
    }



    private string ReplaceVariable(string variableName, string value, string template, bool overrideStrict = false)
    {
        return _templateVariableRegex.Replace(template, (m) =>
        {
            if (m.Groups.TryGetValue("name", out var nameGroup) && nameGroup.Value == variableName)
            {
                return FormatVariable(value, m.Groups[3].Captures);
            }

            if (overrideStrict || Strict) throw new Exception("");

            return m.Name;
        });
    }



    private string FormatVariable(string value, CaptureCollection formats)
    {
        for (int i = 0; i < formats.Count; i++)
        {
            var format = formats[i];

            if (format is not null)
            {
                switch (format.Value.ToLower())
                {
                    case "lower":
                        value = value.ToLower();
                        break;
                    default:
                        if (Strict) throw new Exception("Invalid format.");
                        break;
                }
            }
        }

        return value;
    }
}
