
namespace ProjectLogging.Models.Website;



public record FrontPageModel(string Name, (string Url, string Text)[] WebLinks, string BioFilePath);
