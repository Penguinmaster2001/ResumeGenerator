
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Website;



public class NavLinksModel : IModel
{
    public List<string> PagesToLink { get; } = [];



    public NavLinksModel(List<string> pagesToLink)
    {
        PagesToLink = pagesToLink;
    }




    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
