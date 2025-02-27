
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
                column.Item().PaddingVertical(2.0f).LineHorizontal(2.0f);
            });
    }



    private void Name(IContainer container) => container.Text(NameText).FontSize(20.0f).FontColor(Colors.Blue.Accent1);



    private void ContactRow(IContainer container) => container.Row(row =>
        {
            row.AutoItem().Text(PhoneNumberText);
            row.AutoItem().PaddingHorizontal(2.0f).LineVertical(1.0f);
            row.AutoItem().Text(text => text.Hyperlink(EmailText, $"mailto:{EmailText}").Underline().FontColor(Colors.Blue.Darken2));
            row.AutoItem().PaddingHorizontal(2.0f).LineVertical(1.0f);
            row.AutoItem().Text(LocationText);
        });




    private void URLRow(IContainer container) => container.Row(row =>
        {
            for (int urlIndex = 0; urlIndex < URLs.Count; urlIndex++)
            {
                string url = URLs[urlIndex];
                row.AutoItem().Text(text => text.Hyperlink(url, $"https://{url}").Underline().FontColor(Colors.Blue.Darken2));

                if (urlIndex < URLs.Count)
                {
                    row.AutoItem().PaddingHorizontal(2.0f).LineVertical(1.0f);
                }
            }
        });
}
