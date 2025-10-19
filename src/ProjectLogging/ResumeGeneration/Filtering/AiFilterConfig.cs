
namespace ProjectLogging.ResumeGeneration.Filtering;



public record AiFilterConfig(string ModelPath, string VocabPath, string jobDescriptionPath,
    Dictionary<string, int> SegmentTitleEntryCounts, int BulletPointCount, int SkillCount);
