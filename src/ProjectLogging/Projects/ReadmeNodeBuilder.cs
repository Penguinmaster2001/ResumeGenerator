
using System.Collections;
using System.Diagnostics.CodeAnalysis;



namespace ProjectLogging.Projects;



public class ReadmeNodeBuilder
{
    private readonly List<IReadmeNode> _nodes = [];
    private string _title = string.Empty;
    private object? _content = null;
    public readonly int Level;



    public ReadmeNodeBuilder(int level)
    {
        Level = level;
    }



    public ReadmeNodeBuilder Title(string title)
    {
        _title = title.Trim();
        return this;
    }



    public ReadmeNodeBuilder ParseContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return Content(content);

        return content[0] switch
        {
            '-' => Content(List(content)),
            '|' => Content(Table(content)),
            _ => Content(content),
        };
    }


    private ReadmeTable Table(string content) => new(content);



    private List<string> List(string content)
    {
        return [.. content.Split('\n').Select(l => l.Trim().Trim('-').Trim())];
    }



    public ReadmeNodeBuilder Content<T>(T content)
    {
        _content = content;
        return this;
    }



    public ReadmeNodeBuilder AddChild(IReadmeNode node)
    {
        _nodes.Add(node);
        return this;
    }



    public IReadmeNode Build()
    {
        return _content switch
        {
            List<string> list => new ReadmeNode<List<string>>(list, _title, Level, _nodes),
            ReadmeTable table => new ReadmeNode<ReadmeTable>(table, _title, Level, _nodes),
            _ => new ReadmeNode<string>(_content?.ToString()?.Trim() ?? string.Empty, _title, Level, _nodes),
        };
    }
}



public class ReadmeTable : IEnumerable<ReadmeTableRow>, IEnumerable
{
    public readonly List<string> Categories;
    public List<string[]> Rows;
    private readonly Dictionary<string, int> _columns;



    public ReadmeTable(string content)
    {
        var lines = content.Split('\n');
        Categories = [.. lines[0].Split('|').Select(v => v.Trim()).Where(v => !string.IsNullOrWhiteSpace(v))];

        _columns = [];
        for (int i = 0; i < Categories.Count; i++)
        {
            _columns.Add(Categories[i], i);
        }

        Rows = [.. lines[1..]
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l) && l.StartsWith('|'))
            .Select(l => l.Split('|').Select(v => v.Trim()).Where(v => !string.IsNullOrWhiteSpace(v)))
            .Where(l => !l.All(v => v == "-" || string.IsNullOrWhiteSpace(v)))
            .Select(l => l.ToArray())];
    }



    public IEnumerator<ReadmeTableRow> GetEnumerator()
        => Rows.Select(r => new ReadmeTableRow(_columns, r)).GetEnumerator();



    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



    public override string ToString()
    {
        return $"| {string.Join(" | ", Categories)} |\n{string.Join("\n", Rows.Select(r => $"| {string.Join(" | ", r)} |"))}\n";
    }
}



public class ReadmeTableRow : IReadOnlyDictionary<string, string>, IReadOnlyDictionary<string, object>
{
    private readonly Dictionary<string, int> _columns;
    private readonly string[] _row;

    public string this[string key]
    {
        get => _row[_columns[key]];
    }
    object IReadOnlyDictionary<string, object>.this[string key] { get => this[key]; }

    public IEnumerable<string> Keys { get => _columns.Keys; }
    public IEnumerable<string> Values { get => _row; }
    IEnumerable<object> IReadOnlyDictionary<string, object>.Values { get => Values; }
    public int Count { get => _columns.Count; }


    public ReadmeTableRow(Dictionary<string, int> columns, string[] row)
    {
        _columns = columns;
        _row = row;
    }



    public bool ContainsKey(string key) => _columns.ContainsKey(key);



    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _columns.Select(kvp => new KeyValuePair<string, string>(kvp.Key, _row[kvp.Value])).GetEnumerator();



    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        if (_columns.TryGetValue(key, out var index))
        {
            value = _row[index];
            return true;
        }

        value = default;
        return false;
    }
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
    {
        if (TryGetValue(key, out string? stringValue))
        {
            value = stringValue;
            return true;
        }

        value = default;
        return false;
    }



    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        => _columns.Select(kvp => new KeyValuePair<string, object>(kvp.Key, _row[kvp.Value])).GetEnumerator();



    public override string ToString()
    {
        return $"| {string.Join(" | ", Keys)} |\n{$"| {string.Join(" | ", _row)} |"}";
    }
}
