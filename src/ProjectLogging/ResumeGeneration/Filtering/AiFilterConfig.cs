
namespace ProjectLogging.ResumeGeneration.Filtering;



public record AiFilterConfig(string CrossEncoderModelPath, string CrossEncoderVocabPath,
    string EmbeddingModelPath, string EmbeddingVocabPath,
    string jobDescriptionPath,
    int DefaultEntryCount,
    Dictionary<string, int> SegmentTitleEntryCounts,
    int DefaultPointCount,
    Dictionary<string, int> SegmentTitlePointCounts,
    Dictionary<string, float> EntryBoosts);
