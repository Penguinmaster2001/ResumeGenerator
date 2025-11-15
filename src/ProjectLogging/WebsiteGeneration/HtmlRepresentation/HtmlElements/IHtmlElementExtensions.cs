
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public static class IHtmlElementExtensions
{
    public static IHtmlElement AddCssClass(this IHtmlElement element, string cssClass)
    {
        int classAttributeIndex = element.Tag.Attributes.FindIndex(a => a.Name == "class");
        if (classAttributeIndex == -1)
        {
            return element.AddAttribute("class", cssClass);
        }

        element.Tag.Attributes[classAttributeIndex] = new TagAttribute("class", element.Tag.Attributes[classAttributeIndex].Value + " " + cssClass);
        return element;
    }
}