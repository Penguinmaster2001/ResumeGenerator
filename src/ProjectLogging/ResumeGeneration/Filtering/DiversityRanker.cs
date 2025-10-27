
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
    private readonly float[] _jobDescriptionEmbedding;
    public float Lambda { get; set; } = 0.7f;



    public DiversityRanker(AiFilterConfig config)
    {
        _config = config;

        using var jobDescriptionFile = File.OpenText(_config.jobDescriptionPath);
        var jobDescription = jobDescriptionFile.ReadToEnd();
        _crossScorer = new CrossEncodingScorer(new CrossEncoder(_config.CrossEncoderModelPath, _config.CrossEncoderVocabPath, 512), jobDescription);
        _embeddingGenerator = new EmbeddingGenerator(_config.EmbeddingModelPath, _config.EmbeddingVocabPath);
        _jobDescriptionEmbedding = _embeddingGenerator.GetEmbedding($"Job description: {jobDescription}");

        _promptFactory = new ViewFactory<string>();
        _promptFactory.AddStrategy<ResumeEntryPromptViewStrategy>();
    }



    public List<ResumeSegmentModel> FilterResume(List<ResumeSegmentModel> resumeSegments)
    {
        resumeSegments = [.. resumeSegments];

        // Hardcoding this for now
        // This entire file is potentially the worst code I've ever written

        // Choose 4 best courses
        var educationSegmentIndex = resumeSegments.FindIndex(s => s.TitleText == "education");
        var educationSegment = resumeSegments[educationSegmentIndex];

        var educationSegmentEntries = educationSegment.Entries.ToList();
        var relevantCoursesEntry = educationSegmentEntries.Find(e => e.TitleText == "Relevant Courses")!;

        relevantCoursesEntry.PointsText = [.. relevantCoursesEntry.PointsText.Select(p => (score: MathHelpers.CosineSimilarity(_jobDescriptionEmbedding, _embeddingGenerator.GetEmbedding($"University course taken: {p}")), text: p)).OrderByDescending(p => p.score).Take(4).Select(p => p.text)];

        resumeSegments[educationSegmentIndex] = new(educationSegment.TitleText, educationSegmentEntries);

        // Choose best skills, limit to 90 chars in length
        var skillsSegmentIndex = resumeSegments.FindIndex(s => s.TitleText == "tech skills");
        var skillsSegment = resumeSegments[skillsSegmentIndex];
        var skillsSegmentEntries = skillsSegment.Entries.ToList();

        var bestSkillCategories = skillsSegmentEntries.Select(s => (score: MathHelpers.CosineSimilarity(_jobDescriptionEmbedding, _embeddingGenerator.GetEmbedding($"Skill category: {s.TitleText}: {string.Join(", ", s.PointsText)}")), category: s)).OrderByDescending(s => s.score).Take(4).Select(s => s.category).ToList();

        foreach (var skillCategory in bestSkillCategories)
        {
            int totalLength = skillCategory.TitleText.Length + 2;
            skillCategory.PointsText = [.. skillCategory.PointsText.Select(s => (score: MathHelpers.CosineSimilarity(_jobDescriptionEmbedding, _embeddingGenerator.GetEmbedding($"Skill in category: {skillCategory.TitleText}: {s}")), text: s))
                .OrderByDescending(s => s.score)
                .TakeWhile(s =>
                {
                    totalLength += s.text.Length + 2;
                    return totalLength <= 120;
                })
                .Select(s => s.text)];
        }
        resumeSegments[skillsSegmentIndex] = new(skillsSegment.TitleText, bestSkillCategories);


        var ranks = new List<DiversityRank>();
        var segmentDatabase = new Dictionary<int, (int order, ResumeSegmentModel segment)>();
        var entryDatabase = new Dictionary<int, ResumeEntryModel>();

        var numToTake = new Dictionary<int, int>();

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

                var newRank = CreateDiversityRank($"Resume section title: {segment.TitleText}\nEntry title and summary: {entry.CreateView(_promptFactory)}", entryId, segmentId, multiplier);

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
        var jobScore = _crossScorer.Score(prompt);
        var embedding = _embeddingGenerator.GetEmbedding(prompt);

        var rank = new DiversityRank(jobScore, 0.0f, prompt, embedding, multiplier)
        {
            Id = id,
            Category = category
        };

        return rank;
    }



    private List<DiversityRank> SortRanks(List<DiversityRank> ranks, Dictionary<int, int> categoryNum)
    {
        var unsortedRanks = ranks.ToList();
        var sortedRanks = new List<DiversityRank>();
        var categoryLeft = categoryNum.ToDictionary();

        PreprocessRanks(unsortedRanks);

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
                var similarity = 0.5f * (1.0f + MathHelpers.CosineSimilarity(unsortedRank.Embedding, rankToAdd.Embedding));

                CalculateAverage(unsortedRank, similarity, unsortedRanks.Count);

                unsortedRank.TotalScore = CalculateTotalScore(unsortedRank);
            }

            Console.WriteLine($"{rankToAdd}\n");
        }

        return sortedRanks;
    }



    private void PreprocessRanks(List<DiversityRank> ranks)
    {
        var lowestScore = ranks.Min(r => r.JobScore);
        var highestScore = ranks.Max(r => r.JobScore);
        var averageJobScore = ranks.Average(r => r.JobScore);
        var variance = 0.0f;
        foreach (var rank in ranks)
        {
            variance += (rank.JobScore - averageJobScore) * (rank.JobScore - averageJobScore);
        }

        variance = MathF.Sqrt(variance / (ranks.Count - 1));

        Console.WriteLine($"Min: {lowestScore}\taverageJobScore: {averageJobScore}\tMax: {highestScore}\tvariance: {variance}\n");

        foreach (var rank in ranks)
        {
            rank.JobScore = NormalizeScore((rank.JobScore - averageJobScore) / variance);

            rank.TotalScore = CalculateTotalScore(rank);
        }
    }



    private float NormalizeScore(float score) => 0.5f * (1.0f + MathF.Tanh(score));



    private void CalculateAverage(DiversityRank rank, float added, int num)
    {
        // Max
        // rank.AverageSimilarity = rank.AverageSimilarity > added ? rank.AverageSimilarity : added;

        // Arithmetic average
        // rank.TotalSimilarity += added;
        // rank.AverageSimilarity = rank.TotalSimilarity / num;

        // P-Mean
        var p = 10.0f;
        rank.TotalSimilarity += MathF.Pow(added, p);
        rank.AverageSimilarity = MathF.Pow(rank.TotalSimilarity / num, 1.0f / p);
    }



    private float CalculateTotalScore(DiversityRank rank)
    {
        return rank.Multiplier * ((Lambda * rank.JobScore) - ((1.0f - Lambda) * rank.AverageSimilarity));
    }
}
