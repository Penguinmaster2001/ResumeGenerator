

using System.Diagnostics.CodeAnalysis;
using System.Text;



namespace ProjectLogging.Projects;



public static class ProjectReadmeParser
{
    public static async Task<ProjectReadmeParseResult> ParseReadmeAsync(string path)
    {
        using var reader = new StreamReader(path);

        await GetContent(reader);

        var titleLine = await reader.ReadLineAsync() ?? throw new Exception("Invalid readme.");
        int currentLevel = titleLine.TakeWhile(c => c == '#').Count();
        if (currentLevel != 1) throw new Exception("Invalid readme.");

        var rootBuilder = new ReadmeNodeBuilder(1);
        rootBuilder.Title(titleLine[2..])
            .Content(await GetContent(reader));

        var nodeStack = new Stack<ReadmeNodeBuilder>();
        nodeStack.Push(rootBuilder);

        while (!reader.EndOfStream)
        {
            currentLevel = titleLine.TakeWhile(c => c == '#').Count();

            if (currentLevel <= 1) throw new Exception("Invalid readme.");

            while (currentLevel <= nodeStack.Peek().Level)
            {
                nodeStack.Peek().AddChild(nodeStack.Pop().Build());
            }

            nodeStack.Push(new ReadmeNodeBuilder(currentLevel)
                .Title(titleLine.TrimStart('#').Trim())
                .Content(await GetContent(reader)));
        }

        while (nodeStack.Count > 0)
        {
            nodeStack.Peek().AddChild(nodeStack.Pop().Build());
        }

        return new(nodeStack.Pop().Build());
    }



    private static async Task<string> GetContent(StreamReader stream)
    {
        var content = new StringBuilder();

        while (stream.Peek() != '#' && await stream.ReadLineAsync() is string line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            line = line.Trim();

            content.Append(line);
        }

        return content.ToString();
    }
}



public class ProjectReadmeParseResult(IReadmeNode root)
{
    public IReadmeNode Root { get; } = root;




    public bool TryFindSection<T>(string title, [NotNullWhen(true)] out IReadmeNode<T>? foundNode, StringComparison comparison = StringComparison.CurrentCulture)
    {
        var searchQueue = new Queue<IReadmeNode>();
        searchQueue.Enqueue(Root);

        while (searchQueue.TryDequeue(out var node))
        {
            if (string.Equals(node.Title, title, comparison) && node is IReadmeNode<T> typedReadmeNode)
            {
                foundNode = typedReadmeNode;
                return true;
            }

            foreach (var child in node.Nodes)
            {
                searchQueue.Enqueue(child);
            }
        }

        foundNode = null;
        return false;
    }
}



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
        _title = title;
        return this;
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
            _ => new ReadmeNode<string>(_content?.ToString() ?? string.Empty, _title, Level, _nodes),
        };
    }
}



public class ReadmeNode<T> : IReadmeNode<T>
{
    public T Content { get; }
    public string Title { get; }
    public int Level { get; }



    private readonly List<IReadmeNode> _nodes;
    public IReadOnlyList<IReadmeNode> Nodes => _nodes;



    public ReadmeNode(T content, string title, int level, List<IReadmeNode> nodes)
    {
        Content = content;
        Title = title;
        Level = level;
        _nodes = nodes;
    }



    public override string ToString()
    {
        return $"{(Level - 1) * '\t'}ReadmeNode(Level: {Level}, Title: {Title}, Child Count: {Nodes.Count}, Content: {Content}, Children: {string.Join('\n', Nodes)})";
    }
}



public interface IReadmeNode
{
    string Title { get; }
    int Level { get; }



    IReadOnlyList<IReadmeNode> Nodes { get; }
}



public interface IReadmeNode<out T> : IReadmeNode
{
    T Content { get; }
}
