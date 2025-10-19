
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
        var filteredSegments = new List<ResumeSegmentModel>();

        var defaultEntryCount = _config.DefaultEntryCount <= -1 ? int.MaxValue : _config.DefaultEntryCount;
        var defaultPointCount = _config.DefaultPointCount <= -1 ? int.MaxValue : _config.DefaultPointCount;

        foreach (var resumeSegment in resumeSegments)
        {
            var entryCount = _config.SegmentTitleEntryCounts.GetValueOrDefault(resumeSegment.TitleText, defaultEntryCount);
            var pointCount = _config.SegmentTitlePointCounts.GetValueOrDefault(resumeSegment.TitleText, defaultPointCount);

            var filteredEntries = resumeSegment.Entries;
            if (resumeSegment.Entries.Count > entryCount)
            {
                filteredEntries = [.. resumeSegment.Entries
                                    .Select(e => (score: scorer.Score(e.CreateView(_promptFactory)), entry: e))
                                    .OrderByDescending(x => x.score)
                                    .Take(entryCount)
                                    .Select(x => x.entry)];
            }

            for (int i = 0; i < filteredEntries.Count; i++)
            {
                if (filteredEntries[i].PointsText.Count <= pointCount)
                {
                    continue;
                }

                filteredEntries[i] = ResumeEntryFactory.DuplicateEntry(filteredEntries[i]);
                filteredEntries[i].PointsText = [.. filteredEntries[i].PointsText
                    .Select(t => (score: scorer.Score(t), text: t))
                    .OrderByDescending(x => x.score)
                    .Take(pointCount)
                    .Select(x => x.text)];;
            }

            filteredSegments.Add(new ResumeSegmentModel(resumeSegment.TitleText, filteredEntries));
        }

        return filteredSegments;
    }
}
