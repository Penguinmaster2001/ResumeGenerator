
namespace ProjectLogging.ResumeGeneration.Filtering;



public class DiversityRank(float jobScore,
    float totalScore,
    string prompt,
    float[] embedding,
    int id,
    int category,
    float multiplier = 1.0f) : IComparable<DiversityRank>
{
    public float JobScore { get; set; } = jobScore;
    public float MaxSimilarity { get; set; } = -1.0f;
    public float TotalScore { get; set; } = totalScore;
    public float Multiplier { get; set; } = multiplier;
    public string Prompt { get; set; } = prompt;
    public float[] Embedding { get; set; } = embedding;
    public int Id { get; set; } = id;
    public int Category { get; set; } = category;



    public int CompareTo(DiversityRank? other)
    {
        if (other == null) return 0;

        return TotalScore.CompareTo(other.TotalScore);
    }



    public override string ToString()
    {
        return $"JobScore: {JobScore:0.000}, TotalScore: {TotalScore:0.000},"
            + $"MaxSimilarity: {MaxSimilarity: 0.000}, Prompt: {Prompt}";
    }
}
