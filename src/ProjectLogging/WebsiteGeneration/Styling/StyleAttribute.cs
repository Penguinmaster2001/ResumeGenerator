
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;



namespace ProjectLogging.WebsiteGeneration.Styling;



public class StyleAttribute() : ITagAttribute
{
    public StyleCollection Styles { get; set; } = new();
    public string Name { get => "style"; }
    public string Value { get => string.Join(';', Styles.PropertyValues.Select(pv => $"{pv.property}:{pv.value}")); }



    public StyleAttribute(StyleCollection styles) : this()
    {
        Styles = styles;
    }



    public (string Name, string Value) GetNameValue() => (Name, Value);
}
