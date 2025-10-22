
namespace ProjectLogging.ResumeGeneration.Filtering;



public class EmbeddingScorer : IResumeScorer
{
    public EmbeddingGenerator EmbeddingGenerator { get; private set; }

    private float[] _jobDescriptionEmbed = [];
    private string _jobDescription = string.Empty;
    public string JobDescription
    {
        get => _jobDescription;

        set
        {
            if (string.IsNullOrWhiteSpace(value) || _jobDescription == value)
            {
                return;
            }

            _jobDescription = value.Trim().ToLower();
            _jobDescriptionEmbed = EmbeddingGenerator.GetEmbedding(_jobDescription);
        }
    }



    public EmbeddingScorer(EmbeddingGenerator embeddingGenerator, string jobDescription)
    {
        EmbeddingGenerator = embeddingGenerator;
        JobDescription = jobDescription;
    }



    public float Score(string entryText)
    {
        return Score(EmbeddingGenerator.GetEmbedding(entryText));
    }



    public float Score(float[] entryEmbed) => MathHelpers.CosineSimilarity(_jobDescriptionEmbed, entryEmbed);
}
