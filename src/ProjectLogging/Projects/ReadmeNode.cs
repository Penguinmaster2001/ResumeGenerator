
namespace ProjectLogging.Projects;



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
        return $"{new string('\t', Level - 1)}ReadmeNode(Level: {Level}, Title: {Title}, Child Count: {Nodes.Count}, Content: {Content}{(Nodes.Count > 0 ? $", Children:\n{string.Join('\n', Nodes)}" : string.Empty)})";
    }
}
