
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public interface ITagAttribute
{
    string Name { get; }
    string Value { get; }

    (string Name, string Value) GetNameValue();
}
