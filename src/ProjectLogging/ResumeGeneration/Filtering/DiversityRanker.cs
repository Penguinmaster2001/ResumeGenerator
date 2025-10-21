
using System.Text.Json;
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.Text;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.ResumeGeneration.Filtering;



public class DiversityRanker
{
    public ResumeModel FilterResume(ResumeModel model, string configPath)
    {
        AiFilterConfig? config;
        try
        {
            config = JsonSerializer.Deserialize<AiFilterConfig>(File.OpenRead(configPath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing config: {ex}");
            config = null;
        }

        if (config is null)
        {
            Console.WriteLine("Unable to load ai config, no filtering done");
            return model;
        }

        using var jobDescriptionFile = File.OpenText(config.jobDescriptionPath);
        var jobDescription = jobDescriptionFile.ReadToEnd();

        // Use this to compare with the job description
        var crossScorer = new CrossEncodingScorer(new CrossEncoder(config.ModelPath, config.VocabPath, 512), jobDescription);

        // Use this to compare the similarity between entries
        var embeddingGenerator = new EmbeddingGenerator("../testing/AiModels/all-MiniLM-L6-v2/model.onnx", "../testing/AiModels/all-MiniLM-L6-v2/vocab.txt");

        var projectSegment = model.ResumeBody.ResumeSegments.Find(s => string.Compare(s.TitleText, "projects", StringComparison.OrdinalIgnoreCase) == 0);

        if (projectSegment is null)
        {
            Console.WriteLine("Unable to find segment, no filtering done");
            return model;
        }

        var promptFactory = new ViewFactory<string>();
        promptFactory.AddStrategy<ResumeEntryPromptViewStrategy>();

        Console.WriteLine("Filtering");

        var ranks = new List<DiversityRank>();
        var entryDatabase = new Dictionary<int, ResumeEntryModel>();

        foreach (var entry in projectSegment.Entries)
        {
            var newRank = CreateDiversityRank($"{projectSegment.TitleText} entry: {entry.CreateView(promptFactory)}");
            entryDatabase.Add(newRank.Id, entry);
            ranks.Add(newRank);
        }

        var selectedEntries = SortRanks(ranks).ToList();

        Console.WriteLine("Filtering done");

        foreach (var entry in selectedEntries)
        {
            Console.WriteLine(entry);
        }

        return model;



        DiversityRank CreateDiversityRank(string prompt, int id, int category)
        {
            var jobScore = NormalizeScore(crossScorer.Score(prompt));
            var embedding = embeddingGenerator.GetEmbedding(prompt);

            return new(jobScore, jobScore, prompt, embedding, id, category);
        }
    }



    private List<DiversityRank> SortRanks(List<DiversityRank> ranks, int numToTake = -1)
    {
        int num = numToTake <= -1 ? ranks.Count : numToTake;

        var sortedRanks = new List<DiversityRank>();

        while (num > 0)
        {
            num--;

            ranks.Sort();
            sortedRanks.Add(ranks.Last());
            ranks.RemoveAt(ranks.Count - 1);

            foreach (var unsortedRank in ranks)
            {
                foreach (var sortedRank in sortedRanks)
                {
                    var similarity = CosineSimilarity(unsortedRank.Embedding, sortedRank.Embedding);
                    if (similarity > unsortedRank.MaxSimilarity)
                    {
                        unsortedRank.MaxSimilarity = similarity;
                    }
                }

                unsortedRank.TotalScore = CalculateTotalScore(unsortedRank);
            }
        }

        return sortedRanks;
    }



    private float NormalizeScore(float score) => MathF.Tanh(score / 6.0f);



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



    private float CalculateTotalScore(DiversityRank rank)
    {
        var l = 0.7f;
        return (l * rank.JobScore) - ((1.0f - l) * rank.MaxSimilarity);
    }
}



public class DiversityRank(float jobScore,
    float totalScore,
    string prompt,
    float[] embedding,
    int id,
    int category) : IComparable<DiversityRank>
{
    public float JobScore = jobScore;
    public float MaxSimilarity = -1.0f;
    public float TotalScore = totalScore;
    public string Prompt = prompt;
    public float[] Embedding = embedding;
    public int Id = id;
    public int Category = category;



    public int CompareTo(DiversityRank? other)
    {
        if (other == null) return 0;

        if (TotalScore < other.TotalScore) return -1;

        if (TotalScore == other.TotalScore) return 0;

        return 1;
    }



    public override string ToString()
    {
        return $"JobScore: {JobScore:0.000}, TotalScore: {TotalScore:0.000}, MaxSimilarity: {MaxSimilarity:0.000}, Prompt: {Prompt}";
    }
}
