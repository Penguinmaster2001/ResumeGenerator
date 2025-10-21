
namespace ProjectLogging.ResumeGeneration.Filtering;

public record AiFilterConfig(string ModelPath, string VocabPath, string jobDescriptionPath,
    int DefaultEntryCount,
    Dictionary<string, int> SegmentTitleEntryCounts,
    int DefaultPointCount,
    Dictionary<string, int> SegmentTitlePointCounts,
    string EmbeddingModelPath,
    string EmbeddingVocabPath);
