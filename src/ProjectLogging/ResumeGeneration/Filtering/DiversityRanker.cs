
using ProjectLogging.Models.Resume;
using ProjectLogging.Views.Text;
using ProjectLogging.Views.ViewCreation;



namespace ProjectLogging.ResumeGeneration.Filtering;



public class DiversityRanker
{
    private readonly AiFilterConfig _config;
    private readonly CrossEncodingScorer _crossScorer;
    private readonly EmbeddingGenerator _embeddingGenerator;
    private readonly ViewFactory<string> _promptFactory;
    public float Lambda { get; set; } = 0.3f;
    public float CrossDivisor { get; set; } = 6.0f;




    public DiversityRanker(AiFilterConfig config)
    {
        _config = config;

        using var jobDescriptionFile = File.OpenText(_config.jobDescriptionPath);
        var jobDescription = jobDescriptionFile.ReadToEnd();
        _crossScorer = new CrossEncodingScorer(new CrossEncoder(_config.CrossEncoderModelPath, _config.CrossEncoderVocabPath, 512), jobDescription);
        _embeddingGenerator = new EmbeddingGenerator(_config.EmbeddingModelPath, _config.EmbeddingVocabPath);

        _promptFactory = new ViewFactory<string>();
        _promptFactory.AddStrategy<ResumeEntryPromptViewStrategy>();
    }



    public List<ResumeSegmentModel> FilterResume(List<ResumeSegmentModel> resumeSegments)
    {
        var ranks = new List<DiversityRank>();
        var segmentDatabase = new Dictionary<int, (int order, ResumeSegmentModel segment)>();
        var entryDatabase = new Dictionary<int, ResumeEntryModel>();

        var numToTake = new Dictionary<int, int>();

        var defaultEntryCount = _config.DefaultEntryCount <= -1 ? int.MaxValue : _config.DefaultEntryCount;

        var orderedSegments = new List<(int order, ResumeSegmentModel segment)>();

        int idCounter = 0;
        for (int i = 0; i < resumeSegments.Count; i++)
        {
            var segment = resumeSegments[i];

            if (!_config.SegmentTitleEntryCounts.TryGetValue(segment.TitleText, out var entryCount))
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

                var multiplier = 1.0f;
                if (_config.EntryBoosts.TryGetValue(entry.TitleText, out var boost))
                {
                    multiplier += 0.01f * boost;
                }

                var newRank = CreateDiversityRank($"{segment.TitleText} entry: {entry.CreateView(_promptFactory)}", entryId, segmentId, multiplier);

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
            if (!_config.SegmentTitlePointCounts.TryGetValue(segment.TitleText, out var pointCount))
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
    }



    private DiversityRank CreateDiversityRank(string prompt, int id, int category, float multiplier = 1.0f)
    {
        var jobScore = NormalizeScore(multiplier * _crossScorer.Score(prompt));
        var embedding = _embeddingGenerator.GetEmbedding(prompt);

        return new(jobScore, jobScore, prompt, embedding, id, category, multiplier);
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
                        similarity = MathHelpers.CosineSimilarity(unsortedRank.Embedding, sortedRank.Embedding);
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



    private float NormalizeScore(float score) => MathF.Tanh(score / CrossDivisor);



    private float CalculateTotalScore(DiversityRank rank)
    {
        return NormalizeScore(rank.Multiplier * ((Lambda * rank.JobScore) - ((1.0f - Lambda) * rank.MaxSimilarity)));
    }
}
