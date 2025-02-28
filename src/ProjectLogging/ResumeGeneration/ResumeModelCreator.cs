
using ProjectLogging.Records;



namespace ProjectLogging.ResumeGeneration;



public class ResumeModelCreator
{
    public PersonalInfo PersonalInfo;

    Dictionary<string, HashSet<string>> Skills;
    Dictionary<string, HashSet<string>> Hobbies;
    Dictionary<string, HashSet<string>> Courses;

    public List<Job> Jobs;
    public List<Project> Projects;
    public List<Volunteer> Volunteers;
    public List<Education> Educations;


    public ResumeModelCreator(PersonalInfo personalInfo, Dictionary<string, HashSet<string>> skills,
                              Dictionary<string, HashSet<string>> hobbies, Dictionary<string, HashSet<string>> courses,
                              List<Job> jobs, List<Project> projects, List<Volunteer> volunteers,
                              List<Education> educations)
    {
        PersonalInfo = personalInfo;

        Skills = skills;
        Hobbies = hobbies;
        Courses = courses;

        Jobs = jobs;
        Projects = projects;
        Volunteers = volunteers;
        Educations = educations;
    }



    public ResumeModel CreateModel()
    {
        ResumeHeaderComponent resumeHeader = new(PersonalInfo);

        List<ResumeBodyComponent> resumeBody = new()
        {
            new("tech skills", Skills.Keys.Select(category
                                            => ResumeEntryFactory.CreateEntry(category, Skills[category]))),
            new("volunteer / extracurricular", Volunteers.Select(v
                                                            => ResumeEntryFactory.CreateEntry(v))),
            new("hobbies", Hobbies.Keys.Select(category
                                        => ResumeEntryFactory.CreateEntry(category, Hobbies[category]))),

            new("education", Educations.Select(e => ResumeEntryFactory.CreateEntry(e))
                                       .Concat(Courses.Keys.Select(category
                                           => ResumeEntryFactory.CreateEntry(category, Courses[category])))),

            new("work experience", Jobs.Select(j => ResumeEntryFactory.CreateEntry(j))),
            new("projects", Projects.Select(p => ResumeEntryFactory.CreateEntry(p))),
        };

        return new(resumeHeader, resumeBody);
    }
}
