
namespace ProjectLogging.Projects;



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
