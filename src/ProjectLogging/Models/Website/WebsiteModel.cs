
namespace ProjectLogging.Models.Website;



public class WebsiteModel
{
    public List<WebsitePageModel> Pages { get; set; } = [];
    public Dictionary<int, List<int>> PageLinks { get; set; } = [];
}
