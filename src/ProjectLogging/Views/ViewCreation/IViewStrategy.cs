
namespace ProjectLogging.Views.ViewCreation;



internal interface IViewStrategy<V>
{
    Type ModelType { get; }
}
