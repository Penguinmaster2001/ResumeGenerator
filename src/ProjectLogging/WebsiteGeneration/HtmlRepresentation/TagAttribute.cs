
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public class TagAttribute : ITagAttribute
{

    public string Name { get; set; }
    public string Value { get; set; }



    public TagAttribute(string name, string value)
    {
        Name = name;
        Value = value;
    }



    public (string Name, string Value) GetNameValue() => (Name, Value);
}
