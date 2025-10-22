
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
    // DESIGN ISSUE: Public mutable properties on a class with complex internal state management.
    // These can be changed at any time, potentially invalidating cached calculations or breaking
    // assumptions made during algorithm execution. Consider making these readonly or private with
    // controlled setters, or pass them as parameters to methods that need them.
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
        // DESIGN ISSUE: This method is doing too much and has very high cyclomatic complexity with deeply
        // nested loops and conditionals. It handles database building, ranking, filtering, and rebuilding
        // all in one method spanning ~100 lines. This makes it difficult to understand, test, and maintain.
        // Consider breaking this into smaller, focused methods like:
        // - BuildEntryDatabase()
        // - RankAndSelectEntries()
        // - FilterBulletPoints()
        // - RebuildSegments()
        // This would improve readability and make the algorithm's steps more explicit.
        var ranks = new List<DiversityRank>();
        var segmentDatabase = new Dictionary<int, (int order, ResumeSegmentModel segment)>();
        var entryDatabase = new Dictionary<int, ResumeEntryModel>();

        var numToTake = new Dictionary<int, int>();

        var defaultEntryCount = _config.DefaultEntryCount <= -1 ? int.MaxValue : _config.DefaultEntryCount;

        var orderedSegments = new List<(int order, ResumeSegmentModel segment)>();

        // DESIGN ISSUE: Using a generic 'idCounter' for multiple different entity types (segments, entries,
        // points) in the same method makes the code harder to reason about. The IDs have no semantic meaning
        // and it's unclear which database they belong to. Consider using separate counter variables
        // (segmentIdCounter, entryIdCounter, pointIdCounter) or a typed ID system to make intent clearer.
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

                var newRank = CreateDiversityRank($"{segment.TitleText} entry: {entry.CreateView(_promptFactory)}", entryId, segmentId);
                ranks.Add(newRank);
            }
        }

        var selectedEntries = SortRanks(ranks, numToTake);
        var pointDatabase = new Dictionary<int, string>();

        // DESIGN ISSUE: Reusing the same dictionaries (numToTake, ranks) for different purposes in
        // sequential phases of the algorithm. After filtering entries, these are cleared and reused
        // for filtering points. This makes the code harder to follow and could lead to bugs if the
        // clear operations are missed. Consider using distinct variable names for each phase (e.g.,
        // entryNumToTake/pointNumToTake, entryRanks/pointRanks) to make the separation explicit.
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



    private DiversityRank CreateDiversityRank(string prompt, int id, int category)
    {
        var jobScore = NormalizeScore(_crossScorer.Score(prompt));
        var embedding = _embeddingGenerator.GetEmbedding(prompt);

        return new(jobScore, jobScore, prompt, embedding, id, category);
    }



    private List<DiversityRank> SortRanks(List<DiversityRank> ranks, Dictionary<int, int> categoryNum)
    {
        // DESIGN ISSUE: This is an O(n³) algorithm in the worst case due to triple nested loops (while loop
        // over unsortedRanks, foreach over unsortedRanks, foreach over sortedRanks). For large datasets,
        // this could have significant performance implications. The similarity memoization helps but doesn't
        // change the fundamental complexity. Consider using more efficient data structures (e.g., priority
        // queue for ranking) or algorithmic approaches to reduce complexity.
        var unsortedRanks = ranks.ToList();
        var sortedRanks = new List<DiversityRank>();
        var categoryLeft = categoryNum.ToDictionary();

        // DESIGN ISSUE: Mutable state shared across loop iterations. The memoizedSimilarities dictionary
        // is populated during the algorithm execution and used for lookups. While this is a valid
        // optimization, it makes the code harder to reason about and test because the behavior depends
        // on accumulated state. Consider if this could be pre-computed or isolated into a separate method.
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
        return (Lambda * rank.JobScore) - ((1.0f - Lambda) * rank.MaxSimilarity);
    }
}
