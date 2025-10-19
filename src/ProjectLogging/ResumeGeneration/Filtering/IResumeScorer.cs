
namespace ProjectLogging.ResumeGeneration.Filtering;



public interface IResumeScorer
{
    string JobDescription { get; set; }



    float Score(string entryText);

    float Score(float[] entryEmbed);
}
