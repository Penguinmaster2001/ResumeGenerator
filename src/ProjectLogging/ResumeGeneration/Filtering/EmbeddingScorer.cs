
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



    public float Score(float[] entryEmbed) => CosineSimilarity(_jobDescriptionEmbed, entryEmbed);



    private static float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0.0f;
        float magA = 0.0f;
        float magB = 0.0f;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / (MathF.Sqrt(magA) * MathF.Sqrt(magB));
    }
}
