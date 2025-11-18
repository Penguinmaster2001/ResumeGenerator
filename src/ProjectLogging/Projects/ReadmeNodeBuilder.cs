
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


    private List<string[]> Table(string content)
    {
        return [.. content.Split('\n').Select(l => l.Split('|').Select(i => i.Trim()).ToArray())];
    }



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
            List<string[]> table => new ReadmeNode<List<string[]>>(table, _title, Level, _nodes),
            _ => new ReadmeNode<string>(_content?.ToString()?.Trim() ?? string.Empty, _title, Level, _nodes),
        };
    }
}