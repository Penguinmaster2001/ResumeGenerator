
using ProjectLogging.Skills;
using ProjectLogging.Projects;



namespace ProjectLogging;



public static class Program
{
    public static void Main()
    {
        List<Project> testProjects = new() {
            new("Test Project",
                "This is a test project",
                new List<string> { "Point 1", "Point 2" },
                new List<Skill> { new("Category", "Skill") },
                "Location",
                new DateOnly(2021, 1, 1),
                new DateOnly(2021, 1, 2)),

            new("Current project",
                    "This project doesn't have an end date",
                    new List<string> { "I create a project with null end date", "This is interpreted as a current project" },
                    new List<Skill> { new("another category", "another skill") },
                    "Location",
                    new DateOnly(2021, 1, 1),
                    null)
        };



        ProjectLoader.WriteProjects(testProjects, "../testing/projects.json");
    }
}
