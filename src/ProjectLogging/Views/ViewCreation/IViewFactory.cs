
namespace ProjectLogging.Views.ViewCreation;



public interface IViewFactory<V>
{
    void AddHelper<T, U>(U helper) where U : T;


    T GetHelper<T>();



    void AddStrategy<T>(ViewStrategy<V, T> strategy);



    V CreateView<T>(T model);
}
