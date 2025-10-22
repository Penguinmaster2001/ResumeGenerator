
using System.Text.Json;
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.Text;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.ResumeGeneration.Filtering;



public class DiversityRanker
{
    public List<ResumeSegmentModel> FilterResume(List<ResumeSegmentModel> resumeSegments, string configPath)
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
            return resumeSegments;
        }

        using var jobDescriptionFile = File.OpenText(config.jobDescriptionPath);
        var jobDescription = jobDescriptionFile.ReadToEnd();

        // Use this to compare with the job description
        var crossScorer = new CrossEncodingScorer(new CrossEncoder(config.ModelPath, config.VocabPath, 512), jobDescription);

        // Use this to compare the similarity between entries
        var embeddingGenerator = new EmbeddingGenerator("../testing/AiModels/all-MiniLM-L6-v2/model.onnx", "../testing/AiModels/all-MiniLM-L6-v2/vocab.txt");


        var promptFactory = new ViewFactory<string>();
        promptFactory.AddStrategy<ResumeEntryPromptViewStrategy>();

        Console.WriteLine("Filtering");

        var ranks = new List<DiversityRank>();
        var segmentDatabase = new Dictionary<int, (int order, ResumeSegmentModel segment)>();
        var entryDatabase = new Dictionary<int, ResumeEntryModel>();

        var numToTake = new Dictionary<int, int>();

        var defaultEntryCount = config.DefaultEntryCount <= -1 ? int.MaxValue : config.DefaultEntryCount;

        var orderedSegments = new List<(int order, ResumeSegmentModel segment)>();

        int idCounter = 0;
        for (int i = 0; i < resumeSegments.Count; i++)
        {
            var segment = resumeSegments[i];

            if (!config.SegmentTitleEntryCounts.TryGetValue(segment.TitleText, out var entryCount))
            {
                // Segment was not mutated
                orderedSegments.Add((i, segment));
                continue;
            }

            int segmentId = idCounter;
            idCounter++;
            // Copy the segment to the database
            segmentDatabase.Add(segmentId, (i, new(segment.TitleText)));

            numToTake.Add(segmentId, entryCount);

            foreach (var entry in segment.Entries)
            {
                int entryId = idCounter;
                idCounter++;
                entryDatabase.Add(entryId, entry);

                var newRank = CreateDiversityRank($"{segment.TitleText} entry: {entry.CreateView(promptFactory)}", entryId, segmentId);
                ranks.Add(newRank);
            }
        }

        var selectedEntries = SortRanks(ranks, numToTake);
        var pointDatabase = new Dictionary<int, string>();

        numToTake.Clear();
        ranks.Clear();

        // Now filter entry bullet points
        for (int i = 0; i < selectedEntries.Count; i++)
        {
            var entryRank = selectedEntries[i];
            var segment = segmentDatabase[entryRank.Category].segment;

            // Note *segment*.TitleText
            if (!config.SegmentTitlePointCounts.TryGetValue(segment.TitleText, out var pointCount))
            {
                // Entry was not mutated
                continue;
            }

            // Copy the Entry to the database
            var oldEntry = entryDatabase[entryRank.Id];
            entryDatabase[entryRank.Id] = ResumeEntryFactory.DuplicateEntry(entryDatabase[entryRank.Id]);
            entryDatabase[entryRank.Id].PointsText = [];

            numToTake.Add(entryRank.Id, pointCount);

            foreach (var point in oldEntry.PointsText)
            {
                int pointId = idCounter;
                idCounter++;
                pointDatabase.Add(pointId, point);
                var newRank = CreateDiversityRank($"{segment.TitleText} bullet point under {entryDatabase[entryRank.Id].TitleText}: {point}", pointId, entryRank.Id);
                ranks.Add(newRank);
            }
        }

        var selectedPoints = SortRanks(ranks, numToTake);

        Console.WriteLine("Filtering done");

        // Rebuild entries
        foreach (var selectedPoint in selectedPoints)
        {
            entryDatabase[selectedPoint.Category].PointsText.Add(pointDatabase[selectedPoint.Id]);
        }

        // Rebuild segments
        foreach (var selectedEntry in selectedEntries)
        {
            segmentDatabase[selectedEntry.Category].segment.Entries.Add(entryDatabase[selectedEntry.Id]);
        }

        var filteredSegments = orderedSegments
            .Concat(segmentDatabase.Values)
            .OrderBy(s => s.order)
            .Select(s => s.segment)
            .ToList();

        return filteredSegments;



        DiversityRank CreateDiversityRank(string prompt, int id, int category)
        {
            var jobScore = NormalizeScore(crossScorer.Score(prompt));
            var embedding = embeddingGenerator.GetEmbedding(prompt);

            return new(jobScore, jobScore, prompt, embedding, id, category);
        }
    }



    private List<DiversityRank> SortRanks(List<DiversityRank> ranks, Dictionary<int, int> categoryNum)
    {
        var unsortedRanks = ranks.ToList();
        var sortedRanks = new List<DiversityRank>();
        var categoryLeft = categoryNum.ToDictionary();

        var memoizedSimilarities = new Dictionary<(int, int), float>();

        while (unsortedRanks.Count > 0)
        {
            unsortedRanks.Sort();
            var rankToAdd = unsortedRanks.Last();
            sortedRanks.Add(rankToAdd);
            unsortedRanks.RemoveAt(unsortedRanks.Count - 1);

            categoryLeft[rankToAdd.Category]--;
            // Took as many from this category as is needed, remove the rest
            if (categoryLeft[rankToAdd.Category] <= 0)
            {
                unsortedRanks.RemoveAll(r => r.Category == rankToAdd.Category);
            }

            foreach (var unsortedRank in unsortedRanks)
            {
                foreach (var sortedRank in sortedRanks)
                {
                    if (!memoizedSimilarities.TryGetValue((unsortedRank.Id, sortedRank.Id), out var similarity)
                        && !memoizedSimilarities.TryGetValue((sortedRank.Id, unsortedRank.Id), out similarity))
                    {
                        similarity = CosineSimilarity(unsortedRank.Embedding, sortedRank.Embedding);
                        memoizedSimilarities.Add((unsortedRank.Id, sortedRank.Id), similarity);
                    }

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
