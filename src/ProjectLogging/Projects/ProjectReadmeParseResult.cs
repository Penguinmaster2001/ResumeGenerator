
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
        foundNode = FindSectionOrNull<T>(title, comparison);

        return foundNode is not null;
    }



    public T? GetSectionContentOrDefault<T>(
        string title,
        T? defaultValue = default,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        var found = FindSectionOrNull<T>(title, comparison);

        if (found is not null) return found.Content;

        return defaultValue;
    }



    public IReadmeNode<T>? FindSectionOrNull<T>(
        string title,
        StringComparison comparison = StringComparison.CurrentCulture)
    {
        if (Root is null) return null;

        var searchQueue = new Queue<IReadmeNode>();
        searchQueue.Enqueue(Root);

        while (searchQueue.TryDequeue(out var node))
        {
            if (string.Equals(node.Title, title, comparison) && node is IReadmeNode<T> typedReadmeNode)
            {
                return typedReadmeNode;
            }

            foreach (var child in node.Nodes)
            {
                searchQueue.Enqueue(child);
            }
        }

        return null;
    }



    public override string ToString()
    {
        return $"ProjectReadmeParseResult(Root: {Root})";
    }
}
