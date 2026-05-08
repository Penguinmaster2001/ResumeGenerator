
using System.Diagnostics.CodeAnalysis;
using System.Reflection;



namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public interface IDataCache
{
    object Value { get; }

    IEnumerable<string> VariableNames { get; }



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



    public static IDataCache Create<K, V>(IReadOnlyDictionary<K, V> dictionaryData)
    {
        return new DictionaryDataCache(dictionaryData.ToDictionary(
            kvp => kvp.Key!.ToString()!,
            kvp => kvp.Value! as object));
    }



    public static IDataCache Create(object data)
    {
        if (data is IReadOnlyDictionary<string, object> dictionaryData)
        {
            return new DictionaryDataCache(dictionaryData);
        }

        if (data is IDataCache dataCache)
        {
            return dataCache;
        }

        return new ObjectDataCache(data);
    }
}



public class ObjectDataCache(object data) : IDataCache
{
    public object Value { get; } = data;

    private readonly List<PropertyInfo> _dataProperties = [.. data.GetType().GetProperties()];
    private readonly Dictionary<string, IDataCache> _cache = [];
    public IEnumerable<string> VariableNames { get => _dataProperties.Select(p => p.Name).Union(_cache.Keys); }



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



public class DictionaryDataCache(IReadOnlyDictionary<string, object> data) : IDataCache
{
    public object Value { get; } = nameof(DictionaryDataCache);
    private readonly IReadOnlyDictionary<string, object> _data = data;
    private readonly Dictionary<string, IDataCache> _cache = [];
    public IEnumerable<string> VariableNames { get => _data.Keys.Union(_cache.Keys); }



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
