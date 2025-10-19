
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.Text;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.ResumeGeneration.Filtering;



public class EmbeddingFilter : IResumeFilter
{
    private readonly AiFilterConfig _config;
    private readonly EmbeddingGenerator _embeddingGenerator;
    private readonly ViewFactory<string> _promptFactory;



    public EmbeddingFilter(AiFilterConfig config)
    {
        _config = config;

        _embeddingGenerator = new EmbeddingGenerator(config.ModelPath, config.VocabPath);

        _promptFactory = new ViewFactory<string>();
        _promptFactory.AddStrategy<ResumeEntryPromptViewStrategy>();
    }



    public List<ResumeSegmentModel> FilterData(List<ResumeSegmentModel> resumeSegments, string jobDescription)
    {
        var scorer = new ResumeRelevanceScorer(_embeddingGenerator, jobDescription);
        var filtered = new List<ResumeSegmentModel>();

        foreach (var resumeSegment in resumeSegments)
        {
            if (!_config.SegmentTitleEntryCounts.TryGetValue(resumeSegment.TitleText, out var count))
            {
                filtered.Add(resumeSegment);
                continue;
            }

            filtered.Add(new ResumeSegmentModel(resumeSegment.TitleText,
                resumeSegment.Entries
                    .Select(e => (score: scorer.Score(e.CreateView(_promptFactory)), entry: e))
                    .OrderByDescending(x => x.score)
                    .Take(count)
                    .Select(x => x.entry)));
        }

        return filtered;
    }
}
