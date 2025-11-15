
using System.Diagnostics.CodeAnalysis;



namespace ProjectLogging.Projects;



public class ProjectReadmeParseResult
{
    public IReadmeNode? Root { get; }



    public bool Successful => Root is not null;



    public bool CheckSuccess([NotNullWhen(true)] out IReadmeNode? root)
    {
        root = Root;
        return Successful;
    }



    private ProjectReadmeParseResult(IReadmeNode? root)
    {
        Root = root;
    }



    public static ProjectReadmeParseResult Success(IReadmeNode root) => new(root);
    public static ProjectReadmeParseResult Failure => new(null);




    public bool TryFindSection<T>(
        string title,
        [NotNullWhen(true)] out IReadmeNode<T>? foundNode,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        if (Root is null)
        {
            foundNode = null;
            return false;
        }

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



    public override string ToString()
    {
        return $"ProjectReadmeParseResult(Root: {Root})";
    }
}
