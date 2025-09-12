
namespace ProjectLogging.Views.ViewCreation;



public interface IViewStrategy<V>
{
    Type ModelType { get; }
}
