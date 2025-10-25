
namespace ProjectLogging.Views.ViewCreation;



public interface IViewFactory<V>
{
    void AddHelper<T, U>() where U : T, new();
    void AddHelper<T>(T helper);
    T GetHelper<T>();



    void AddPostAction(Action<V, IViewFactory<V>> action);



    void AddStrategy<T>(ViewStrategy<V, T> strategy);



    V CreateView<T>(T model);
}
