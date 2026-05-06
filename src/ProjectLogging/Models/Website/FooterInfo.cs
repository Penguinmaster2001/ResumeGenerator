
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Models.Website;



public class FooterInfo : IModel
{
    public string Year { get; set; }



    public FooterInfo(string year)
    {
        Year = year;
    }



    public V CreateView<V>(IViewFactory<V> viewFactory) => viewFactory.CreateView(this);
}
