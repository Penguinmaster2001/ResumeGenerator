
using System.Text;
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.Views.Text;



public class ResumeEntryPromptViewStrategy : ViewStrategy<string, ResumeEntryModel>
{
    public override string BuildView(ResumeEntryModel model, IViewFactory<string> factory)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{model.TitleText}:");

        if (model.DescriptionText is not null)
        {
            sb.AppendLine(model.DescriptionText);
        }

        foreach (var point in model.BulletPointsText)
        {
            sb.Append("- ").AppendLine(point);
        }

        return sb.ToString();
    }
}
