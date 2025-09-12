
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation;



public interface ITagAttribute
{
    (string Name, string Value) GetNameValue();
}
