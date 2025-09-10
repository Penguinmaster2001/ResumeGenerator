
namespace ProjectLogging.Views.ViewCreation;



public abstract class ViewStrategy<V, T> : IViewStrategy<V>
{
    public Type ModelType => typeof(T);



    public abstract V BuildView(T model, IViewFactory<V> factory);
}
