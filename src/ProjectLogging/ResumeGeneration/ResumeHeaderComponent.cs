
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public class ResumeHeaderComponent : IComponent
{
    public string NameText;

    public string PhoneNumberText;
    public string EmailText;
    public string LocationText;

    public List<string> URLs;



    public ResumeHeaderComponent(string name, string phoneNumber, string email, string location, List<string> urls)
    {
        NameText = name;
        PhoneNumberText = phoneNumber;
        EmailText = email;
        LocationText = location;
        URLs = urls;
    }



    public ResumeHeaderComponent(PersonalInfo personalInfo)
    {
        NameText = personalInfo.Name;
        PhoneNumberText = personalInfo.PhoneNumber;
        EmailText = personalInfo.Email;
        LocationText = personalInfo.Location;
        URLs = personalInfo.URLs;
    }



    public void Compose(IContainer container)
    {
        container.Column(column =>
            {
                column.Item().Element(Name);
                column.Item().Element(ContactRow);
                column.Item().Element(URLRow);
            });
    }



    private void Name(IContainer container) => container.Text(NameText).FontSize(20.0f).FontColor(Colors.Blue.Accent1);



    private void ContactRow(IContainer container) => container.Text(text =>
        {
            text.Span(PhoneNumberText);
            text.Span("|");
            text.Hyperlink(EmailText, $"mailto:{EmailText}").Underline().FontColor(Colors.Blue.Darken2);
            text.Span("|");
            text.Span(LocationText);
        });




    private void URLRow(IContainer container) => container.Text(text =>
        {
            for (int urlIndex = 0; urlIndex < URLs.Count; urlIndex++)
            {
                string url = URLs[urlIndex];
                text.Hyperlink(url, url).Underline().FontColor(Colors.Blue.Darken2);

                if (urlIndex < URLs.Count)
                {
                    text.Span("|");
                }
            }
        });
}
