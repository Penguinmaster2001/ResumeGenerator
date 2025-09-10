
namespace ProjectLogging.Views.ViewCreation;



public interface IViewFactory<V>
{
    void AddStrategy<T>(ViewStrategy<V, T> strategy);



    V BuildView<T>(T model);
}
