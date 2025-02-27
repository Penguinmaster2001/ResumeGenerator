
using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public class ResumeModelCreator
{
    public PersonalInfo PersonalInfo;
    public List<Job> Jobs;
    public List<Project> Projects;
    public List<Volunteer> Volunteers;


    public ResumeModelCreator(PersonalInfo personalInfo, List<Job> jobs, List<Project> projects, List<Volunteer> volunteers)
    {
        PersonalInfo = personalInfo;
        Jobs = jobs;
        Projects = projects;
        Volunteers = volunteers;
    }



    public ResumeModel CreateModel()
    {
        ResumeHeaderComponent resumeHeader = new(PersonalInfo);

        List<ResumeBodyComponent> resumeBody = new()
        {
            new("volunteer / extracurricular", Volunteers.Select(v => ResumeEntryFactory.CreateEntry(v))),
            new("work experience", Jobs.Select(j => ResumeEntryFactory.CreateEntry(j))),
            new("projects", Projects.Select(p => ResumeEntryFactory.CreateEntry(p))),
        };

        return new(resumeHeader, resumeBody);
    }
}
