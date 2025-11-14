

using System.Diagnostics.CodeAnalysis;



namespace ProjectLogging.Projects;



public static class ProjectReadmeParser
{

}



public class ProjectReadmeParseResult
{
    public string Title { get; }
    public string Description { get; }

    public IReadmeNode Root { get; }




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



public class ReadmeNode<T> : IReadmeNode<T>
{
    public T Content { get; }
    public string Title { get; }



    private readonly List<IReadmeNode> _nodes;
    public IReadOnlyList<IReadmeNode> Nodes => _nodes;



    public ReadmeNode(T content, string title, List<IReadmeNode> nodes)
    {
        Content = content;
        Title = title;
        _nodes = nodes;
    }
}



public interface IReadmeNode
{
    string Title { get; }



    IReadOnlyList<IReadmeNode> Nodes { get; }
}



public interface IReadmeNode<out T> : IReadmeNode
{
    T Content { get; }
}
