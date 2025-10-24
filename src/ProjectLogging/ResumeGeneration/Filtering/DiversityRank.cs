
namespace ProjectLogging.ResumeGeneration.Filtering;



public class DiversityRank(float jobScore,
    float totalScore,
    string prompt,
    float[] embedding,
    float multiplier = 1.0f) : IComparable<DiversityRank>
{
    public float JobScore { get; set; } = jobScore;
    public float AverageSimilarity { get; set; } = 0.0f;
    public float TotalSimilarity { get; set; } = 0.0f;
    public float TotalScore { get; set; } = totalScore;
    public float Multiplier { get; set; } = multiplier;
    public string Prompt { get; } = prompt;
    public float[] Embedding { get; set; } = embedding;
    public int Id { get; set; }
    public int Category { get; set; }



    public int CompareTo(DiversityRank? other)
    {
        if (other == null) return 0;

        return TotalScore.CompareTo(other.TotalScore);
    }



    public override string ToString()
    {
        return $"JobScore: {JobScore:0.000}, TotalScore: {TotalScore:0.000}, "
            + $"AverageSimilarity: {AverageSimilarity: 0.000}, Prompt: {Prompt}";
    }
}




public class DiversityRankCategory(int order, int id, int numToTake)
{
    public int Order { get; } = order;
    public int Id { get; } = id;
    public int NumToTake { get; set; } = numToTake;
}




public class DiversityRankCollection<C, S>()
{
    public List<DiversityRank> Ranks = [];
    public Dictionary<int, DiversityRankCategory> Categories = [];
    public Dictionary<int, C> CategoryDatabase = [];
    public Dictionary<int, S> SortablesDatabase = [];



    public DiversityRankCollection(List<C> categories,
            Func<C, IEnumerable<S>> extractSortables,
            Func<C, int> categoryNumToTake,
            Func<C, S, DiversityRank> createDiversityRank)
        : this()
    {
        int idCounter = 0;
        for (int i = 0; i < categories.Count; i++)
        {
            int categoryId = idCounter++;
            var categoryData = categories[i];
            var numToTake = categoryNumToTake(categoryData);
            var category = new DiversityRankCategory(i, categoryId, numToTake);
            CategoryDatabase.Add(categoryId, categoryData);
            Categories.Add(categoryId, category);

            if (numToTake <= 0) continue; // Don't add any sortables

            foreach (var sortableData in extractSortables(categoryData))
            {
                int rankId = idCounter++;
                var rank = createDiversityRank(categoryData, sortableData);
                rank.Id = rankId;
                rank.Category = categoryId;
                SortablesDatabase.Add(rankId, sortableData);
                Ranks.Add(rank);
            }
        }
    }
}
