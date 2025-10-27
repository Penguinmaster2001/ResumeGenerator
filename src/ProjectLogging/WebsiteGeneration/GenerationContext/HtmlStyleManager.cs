
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;
using ProjectLogging.WebsiteGeneration.Styling;



namespace ProjectLogging.WebsiteGeneration.GenerationContext;



public class HtmlStyleManager : IHtmlStyleManager
{
    private readonly Dictionary<IHtmlElement, StyleCollection> _elementStyles = [];



    public StyleCollection GetStyles(IHtmlElement element)
        => _elementStyles.TryGetValue(element, out var styleCollection) ? styleCollection : new();



    public void AddStyle(IHtmlElement element, string property, string value)
    {
        if (!_elementStyles.TryGetValue(element, out var styles))
        {
            styles = new StyleCollection();
            _elementStyles[element] = styles;
        }

        styles.PropertyValues.Add((property, value));
    }



    public void ApplyStyle(IHtmlElement element)
    {
        if (!_elementStyles.TryGetValue(element, out var styleCollection)) return;

        element.AddAttribute(new StyleAttribute(styleCollection));
    }
}
