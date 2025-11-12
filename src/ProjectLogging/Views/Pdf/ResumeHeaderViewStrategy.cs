
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration.Styling;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeHeaderViewStrategy : ViewStrategy<Action<IContainer>, ResumeHeaderModel>
{
    public override Action<IContainer> BuildView(ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => Compose(container, model, factory);



    public void Compose(IContainer container, ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
    {
        container.DefaultTextStyle(style => style.Bold()
                                                 .FontColor(factory.GetHelper<IPdfStyleManager>().ResumeHeaderTextColor)
                                                 .FontSize(13.0f))
                 .Column(column =>
            {
                column.Item().AlignCenter().Element(Name(model, factory));
                // column.Item().AlignCenter().Element(ContactRow(model));
                // column.Item().AlignCenter().Element(URLRow(model));
                column.Item().AlignCenter().Element(CombinedRow(model));
                column.Item().PaddingVertical(3.0f).LineHorizontal(0.5f).LineColor(factory.GetHelper<IPdfStyleManager>().AccentColor);
            });
    }



    private Action<IContainer> Name(ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Text(model.NameText).FontSize(14.0f).FontColor(factory.GetHelper<IPdfStyleManager>().NameTextColor);



    private Action<IContainer> ContactRow(ResumeHeaderModel model)
        => (container) => container.Row(row =>
            {
                row.AutoItem().Text(model.PhoneNumberText);
                row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(text => text.Hyperlink(model.EmailText, $"mailto:{model.EmailText}").Underline());
                row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(model.LocationText);
            });




    private Action<IContainer> URLRow(ResumeHeaderModel model)
        => (container) => container.Row(row =>
            {
                for (int urlIndex = 0; urlIndex < model.URLs.Count; urlIndex++)
                {
                    string url = model.URLs[urlIndex];
                    row.AutoItem().Text(text => text.Hyperlink(url, $"https://{url}").Underline());

                    if (urlIndex < model.URLs.Count - 1)
                    {
                        row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                    }
                }
            });



    private Action<IContainer> CombinedRow(ResumeHeaderModel model)
        => (container) => container.Row(row =>
            {
                var horizontalPadding = 3.0f;
                row.AutoItem().Text(model.PhoneNumberText);
                row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(model.LocationText);
                row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(text => text.Hyperlink(model.EmailText, $"mailto:{model.EmailText}").Underline());
                row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);

                for (int urlIndex = 0; urlIndex < model.URLs.Count; urlIndex++)
                {
                    string url = model.URLs[urlIndex];
                    row.AutoItem().Text(text => text.Hyperlink(url, $"https://{url}").Underline());

                    if (urlIndex < model.URLs.Count - 1)
                    {
                        row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);
                    }
                }
            });
}
