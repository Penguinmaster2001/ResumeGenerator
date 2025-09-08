
using System.Text;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public class HtmlText : IHtmlElement
{
    private static readonly List<HtmlTag.HtmlTags> NonNestingTags = [
        HtmlTag.HtmlTags.Paragraph,
        HtmlTag.HtmlTags.Header1,
        HtmlTag.HtmlTags.Header2,
        HtmlTag.HtmlTags.Header3,
        HtmlTag.HtmlTags.Header4,
        HtmlTag.HtmlTags.Header5,
        HtmlTag.HtmlTags.Header6,
    ];



    private static HtmlText Begin() => new();
    public static HtmlText BeginParagraph(string text = "") => Begin().StartParagraph(text);
    public static HtmlText BeginHeader(int header, string text = "") => Begin().StartHeader(header, text);
    public static HtmlText BeginCode(string text = "") => Begin().Code(text);
    public static HtmlText BeginBold(string text = "") => Begin().Bold(text);
    public static HtmlText BeginEm(string text = "") => Begin().Em(text);



    private readonly Dictionary<HtmlTag.HtmlTags, bool> _modifierStates = new()
    {
        { HtmlTag.HtmlTags.Bold,        false },
        { HtmlTag.HtmlTags.Code,        false },
        { HtmlTag.HtmlTags.Emphasis,    false },
    };



    private readonly List<IHtmlElement> _completedElements = [];
    private readonly StringBuilder _workingText = new();
    private readonly Stack<HtmlTag> _workingTags = [];
    private bool _inNestedTag = false;

    public List<HtmlTag.Attribute> Attributes { get => throw new NotImplementedException(); }




    public HtmlText Text(string text)
    {
        _workingText.Append(text);
        return this;
    }



    public HtmlText StartParagraph(string paragraph = "")
    {
        AddNonNested(HtmlTag.HtmlTags.Paragraph, paragraph);
        return this;
    }



    public HtmlText StartHeader(int header, string text = "")
    {
        AddNonNested(HtmlTag.TextHeader(header), text);

        return this;
    }



    public HtmlText StartBold()
    {
        StartModifier(HtmlTag.HtmlTags.Bold);
        return this;
    }

    public HtmlText EndBold()
    {
        EndModifier(HtmlTag.HtmlTags.Bold);
        return this;
    }

    public HtmlText Bold(string text) => StartBold().Text(text).EndBold();



    public HtmlText StartCode()
    {
        StartModifier(HtmlTag.HtmlTags.Code);
        return this;
    }

    public HtmlText EndCode()
    {
        EndModifier(HtmlTag.HtmlTags.Code);
        return this;
    }

    public HtmlText Code(string text) => StartCode().Text(text).EndCode();



    public HtmlText StartEm()
    {
        StartModifier(HtmlTag.HtmlTags.Emphasis);
        return this;
    }

    public HtmlText EndEm()
    {
        EndModifier(HtmlTag.HtmlTags.Emphasis);
        return this;
    }

    public HtmlText Em(string text) => StartEm().Text(text).EndEm();



    private void StartModifier(HtmlTag.HtmlTags modifier)
    {
        if (!_modifierStates[modifier])
        {
            StartTag(modifier);
            _modifierStates[modifier] = true;
        }
    }



    private void EndModifier(HtmlTag.HtmlTags modifier)
    {
        if (_modifierStates[modifier])
        {
            EndTag(modifier);
            _modifierStates[modifier] = false;
        }
    }



    private void AddNonNested(HtmlTag.HtmlTags tag, string text)
    {
        UnNest();
        _inNestedTag = true;
        AddText(tag, text);
    }



    private void AddText(HtmlTag.HtmlTags tag, string text)
    {
        StartTag(tag);
        _workingText.Append(text);
    }



    private void StartTag(HtmlTag.HtmlTags tag)
    {
        var htmlTag = new HtmlTag(tag);
        _workingTags.Push(htmlTag);
        _workingText.Append(htmlTag.Opener);
    }



    private void EndTag(HtmlTag.HtmlTags tag)
    {
        var toReplace = new Stack<HtmlTag>();

        while (_workingTags.TryPop(out var currentTag))
        {
            _workingText.Append(currentTag.Closer);

            if (currentTag.Tag == tag) break;

            toReplace.Push(currentTag);
        }

        while (toReplace.TryPop(out var replaceTag))
        {
            _workingTags.Push(replaceTag);
            _workingText.Append(replaceTag.Opener);
        }
    }



    private void UnNest()
    {
        if (!_inNestedTag) return;

        while (_workingTags.TryPop(out var currentTag))
        {
            _workingText.Append(currentTag.Closer);

            if (NonNestingTags.Contains(currentTag.Tag)) break;
        }

        _inNestedTag = false;
    }



    private void CloseAllTags()
    {
        while (_workingTags.TryPop(out var currentTag))
        {
            _workingText.Append(currentTag.Closer);
        }
    }



    public IHtmlItem Build()
    {
        CloseAllTags();

        return new RawHtml(_workingText.ToString());
    }




    public IHtmlElement AddAttribute(string name, string value)
    {
        throw new NotImplementedException();
    }

    public IHtmlElement AddAttribute(HtmlTag.Attribute attribute)
    {
        throw new NotImplementedException();
    }



    public string GenerateHtml() => Build().GenerateHtml();
}
