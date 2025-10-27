
namespace ProjectLogging.ResumeGeneration.Filtering;



public class CrossEncodingScorer : IResumeScorer
{
    public CrossEncoder CrossEncoder { get; private set; }

    private long[] _jobTokens = [];
    private string _jobDescription = string.Empty;
    public string JobDescription
    {
        get => _jobDescription;

        set
        {
            if (string.IsNullOrWhiteSpace(value) || _jobDescription == value)
            {
                return;
            }

            _jobDescription = ("How well does the resume snippet match this job description?\n" + value.Trim()).ToLower();

            _jobTokens = [.. CrossEncoder.Tokenizer.EncodeToIds(_jobDescription).Select(i => (long)i)];
        }
    }



    public CrossEncodingScorer(CrossEncoder crossEncoder, string jobDescription)
    {
        CrossEncoder = crossEncoder;
        JobDescription = jobDescription;
    }



    public float Score(string entryText)
    {
        var prompt = CreatePrompt(entryText);
        var score = CrossEncoder.Score(_jobTokens, prompt);
        
        return score;
    }



    private string CreatePrompt(string text)
    {
        return $"Resume snippet:\n{text}";
    }
}
