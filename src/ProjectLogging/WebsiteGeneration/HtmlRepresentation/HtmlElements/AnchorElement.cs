
namespace ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



public class AnchorElement : HtmlSection<AnchorElement>
{
    public enum Targets
    {
        Self,
        Blank,
        Parent,
        Top,
    }



    public static string TargetString(Targets target) => target switch
    {
        Targets.Top => "_top",
        Targets.Parent => "_parent",
        Targets.Blank => "_blank",
        _ => "_self",
    };



    public AnchorElement(string href, params List<IHtmlItem> content)
        : base(HtmlTag.Anchor, content)
        => AddAttribute("href", href).AddAttribute("target", TargetString(Targets.Self));



    public AnchorElement(string href, Targets target, params List<IHtmlItem> content)
        : base(HtmlTag.Anchor, content)
        => AddAttribute("href", href).AddAttribute("target", TargetString(target));
}
