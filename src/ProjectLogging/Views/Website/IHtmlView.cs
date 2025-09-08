
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;



namespace ProjectLogging.Views.Website;



public interface IHtmlView
{
    IHtmlItem GetHtmlItem();
}



public interface IHtmlView<T> : IHtmlView
    where T : IHtmlItem
{
    new T GetHtmlItem();
}
