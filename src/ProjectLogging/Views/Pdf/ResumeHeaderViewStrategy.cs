
using ProjectLogging.Models.Resume;
using ProjectLogging.ResumeGeneration;
using ProjectLogging.Views.ViewCreation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace ProjectLogging.Views.Pdf;



public class ResumeHeaderViewStrategy : ViewStrategy<Action<IContainer>, ResumeHeaderModel>
{
    public override Action<IContainer> BuildView(ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => Compose(container, model, factory);



    public void Compose(IContainer container, ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
    {
        container.DefaultTextStyle(style => style.Bold()
                                                 .FontColor(Colors.Blue.Accent2)
                                                 .FontSize(11.0f))
                 .Column(column =>
            {
                column.Item().AlignCenter().Element(Name(model));
                // column.Item().Element(ContactRow(model));
                // column.Item().Element(URLRow(model));
                column.Item().AlignCenter().Element(CombinedRow(model, factory));
                column.Item().PaddingTop(4.0f).PaddingBottom(3.0f).LineHorizontal(1.0f);
            });
    }



    private Action<IContainer> Name(ResumeHeaderModel model)
        => (container) => container.Text(model.NameText).FontSize(14.0f);



    private Action<IContainer> ContactRow(ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Row(row =>
            {
                row.AutoItem().Text(model.PhoneNumberText);
                row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(text => text.Hyperlink(model.EmailText, $"mailto:{model.EmailText}").Underline().FontColor(factory.GetHelper<IPdfStyleManager>().HeaderTextColor));
                row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(model.LocationText);
            });




    private Action<IContainer> URLRow(ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Row(row =>
            {
                for (int urlIndex = 0; urlIndex < model.URLs.Count; urlIndex++)
                {
                    string url = model.URLs[urlIndex];
                    row.AutoItem().Text(text => text.Hyperlink(url, $"https://{url}").Underline().FontColor(factory.GetHelper<IPdfStyleManager>().HeaderTextColor));

                    if (urlIndex < model.URLs.Count - 1)
                    {
                        row.AutoItem().AlignMiddle().PaddingHorizontal(5.0f).Height(12.0f).LineVertical(0.8f);
                    }
                }
            });



    private Action<IContainer> CombinedRow(ResumeHeaderModel model, IViewFactory<Action<IContainer>> factory)
        => (container) => container.Row(row =>
            {
                var fontColor = factory.GetHelper<IPdfStyleManager>().HeaderTextColor;
                var horizontalPadding = 3.0f;
                row.AutoItem().Text(model.PhoneNumberText);
                row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(model.LocationText);
                row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);
                row.AutoItem().Text(text => text.Hyperlink(model.EmailText, $"mailto:{model.EmailText}").Underline().FontColor(fontColor));
                row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);

                for (int urlIndex = 0; urlIndex < model.URLs.Count; urlIndex++)
                {
                    string url = model.URLs[urlIndex];
                    row.AutoItem().Text(text => text.Hyperlink(url, $"https://{url}").Underline().FontColor(fontColor));

                    if (urlIndex < model.URLs.Count - 1)
                    {
                        row.AutoItem().AlignMiddle().PaddingHorizontal(horizontalPadding).Height(12.0f).LineVertical(0.8f);
                    }
                }
            });
}
