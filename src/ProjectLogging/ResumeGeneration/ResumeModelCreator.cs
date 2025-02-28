
using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public class ResumeModelCreator
{
    public PersonalInfo PersonalInfo;

    Dictionary<string, HashSet<string>> Skills;
    Dictionary<string, HashSet<string>> Hobbies;

    public List<Job> Jobs;
    public List<Project> Projects;
    public List<Volunteer> Volunteers;


    public ResumeModelCreator(PersonalInfo personalInfo, Dictionary<string, HashSet<string>> skills,
                              Dictionary<string, HashSet<string>> hobbies, List<Job> jobs, List<Project> projects,
                              List<Volunteer> volunteers)
    {
        PersonalInfo = personalInfo;

        Skills = skills;
        Hobbies = hobbies;

        Jobs = jobs;
        Projects = projects;
        Volunteers = volunteers;
    }



    public ResumeModel CreateModel()
    {
        ResumeHeaderComponent resumeHeader = new(PersonalInfo);

        List<ResumeBodyComponent> resumeBody = new()
        {
            new("tech skills", Skills.Keys.Select(category => ResumeEntryFactory.CreateEntry(category, Skills[category]))),
            new("volunteer / extracurricular", Volunteers.Select(v => ResumeEntryFactory.CreateEntry(v))),
            new("work experience", Jobs.Select(j => ResumeEntryFactory.CreateEntry(j))),
            new("projects", Projects.Select(p => ResumeEntryFactory.CreateEntry(p))),
            new("hobbies", Hobbies.Keys.Select(category => ResumeEntryFactory.CreateEntry(category, Hobbies[category]))),
        };

        return new(resumeHeader, resumeBody);
    }
}
