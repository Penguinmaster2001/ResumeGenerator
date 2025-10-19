
using ProjectLogging.Data;



namespace ProjectLogging.ResumeGeneration.Filtering;




public class EmbeddingFilter : IResumeFilter
{
    EmbeddingGenerator _embeddingGenerator;


    public EmbeddingFilter(string modelPath, string vocabPath)
    {
        _embeddingGenerator = new EmbeddingGenerator(modelPath, vocabPath);
    }



    public List<(float score, Project project)> FilterData(IDataCollection data, string jobDescription)
    {
        var scorer = new ResumeRelevanceScorer(_embeddingGenerator, jobDescription);
        var filtered = new DataCollection();

        return [.. data.GetData<List<Project>>("projects")
            .Where(p => p.ShortDescription is not null)
            .Select(p => (score: scorer.Score(p.ShortDescription ?? string.Empty), project: p))
            .OrderByDescending(x => x.score)];
    }
}
