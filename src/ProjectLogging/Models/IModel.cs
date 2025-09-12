
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models;



public interface IModel
{
    V CreateView<V>(IViewFactory<V> viewFactory);
}
