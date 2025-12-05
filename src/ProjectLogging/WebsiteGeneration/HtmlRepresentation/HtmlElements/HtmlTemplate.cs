
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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
                    ? throw new Exception($"Unknown variable \"{variableGroup.Value}\".")
                    : m.Value);

                return m.Groups.TryGetValue("formats", out var formats)
                    ? FormatVariable(variableString, formats.Value, false)
                    : variableString;
            }

            if (overrideStrict ?? Strict) throw new Exception($"Unknown variable \"{variableGroup?.Value ?? m.Value}\".");

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



    private interface IDataCache
    {
        object Value { get; }



        public bool TryGetValue(string variable, [NotNullWhen(true)] out object? value)
        {
            var nestedVariables = variable.Split('.');

            IDataCache? currentData = this;

            foreach (var nestedVariable in nestedVariables)
            {
                if (!currentData.TryGetData(nestedVariable, out currentData))
                {
                    value = null;
                    return false;
                }
            }

            value = currentData.Value;
            return true;
        }



        bool TryGetData(string variable, [NotNullWhen(true)] out IDataCache? data);



        IDataCache Extended(string variable, object value);



        public static IDataCache Create(object data)
        {
            if (data is IReadOnlyDictionary<string, object> dictionaryData)
            {
                return new DictionaryDataCache(dictionaryData);
            }

            return new ObjectDataCache(data);
        }
    }



    private class ObjectDataCache(object data) : IDataCache
    {
        public object Value { get; } = data;
        private readonly List<PropertyInfo> _dataProperties = [.. data.GetType().GetProperties()];
        private readonly Dictionary<string, IDataCache> _cache = [];



        private ObjectDataCache(object data, Dictionary<string, IDataCache> cache) : this(data)
        {
            _cache = cache;
        }



        public bool TryGetData(string variable, [NotNullWhen(true)] out IDataCache? data)
        {
            if (_cache.TryGetValue(variable, out data)) return true;

            if (_dataProperties.Find(p => p.Name == variable) is not PropertyInfo variableProperty
                || variableProperty.GetValue(Value) is not object variableValue) return false;

            data = IDataCache.Create(variableValue);
            _cache.Add(variable, data);

            return true;
        }



        public IDataCache Extended(string variable, object value)
        {
            var valueCache = IDataCache.Create(value);

            var extendedCache = _cache.ToDictionary();
            extendedCache.Add(variable, valueCache);

            return new ObjectDataCache(Value, extendedCache);
        }
    }



    private class DictionaryDataCache(IReadOnlyDictionary<string, object> data) : IDataCache
    {
        public object Value { get; } = nameof(DictionaryDataCache);
        private readonly IReadOnlyDictionary<string, object> _data = data;
        private readonly Dictionary<string, IDataCache> _cache = [];



        public bool TryGetData(string variable, [NotNullWhen(true)] out IDataCache? data)
        {
            if (_cache.TryGetValue(variable, out data)) return true;

            if (!_data.TryGetValue(variable, out var variableProperty)) return false;

            data = IDataCache.Create(variableProperty);
            _cache.Add(variable, data);

            return true;
        }



        public IDataCache Extended(string variable, object value)
        {
            var extendedData = _data.ToDictionary();
            extendedData.Add(variable, value);

            return new DictionaryDataCache(extendedData);
        }
    }
}
