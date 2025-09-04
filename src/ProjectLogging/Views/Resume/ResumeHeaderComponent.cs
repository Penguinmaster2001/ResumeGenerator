
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ProjectLogging.Models;



namespace ProjectLogging.Views.Resume;



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
        container.DefaultTextStyle(style => style.Bold()
                                                 .FontColor(Colors.Blue.Accent2)
                                                 .FontSize(14.0f))
                 .Column(column =>
            {
                column.Item().Element(Name);
                column.Item().Element(ContactRow);
                column.Item().Element(URLRow);
                column.Item().PaddingVertical(2.0f).LineHorizontal(1.0f);
            });
    }



    private void Name(IContainer container) => container.Text(NameText).FontSize(20.0f);



    private void ContactRow(IContainer container) => container.Row(row =>
        {
            row.AutoItem().Text(PhoneNumberText);
            row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
            row.AutoItem().Text(text => text.Hyperlink(EmailText, $"mailto:{EmailText}").Underline().FontColor(Colors.Blue.Darken4));
            row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
            row.AutoItem().Text(LocationText);
        });




    private void URLRow(IContainer container) => container.Row(row =>
        {
            for (int urlIndex = 0; urlIndex < URLs.Count; urlIndex++)
            {
                string url = URLs[urlIndex];
                row.AutoItem().Text(text => text.Hyperlink(url, $"https://{url}").Underline().FontColor(Colors.Blue.Darken4));

                if (urlIndex < URLs.Count - 1)
                {
                    row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                }
            }
        });
}
