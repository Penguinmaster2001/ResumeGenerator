
using ProjectLogging.Skills;
using ProjectLogging.Projects;



namespace ProjectLogging;



public static class Program
{
    public static void Main()
    {
        Project testProject = new(
            "Test Project",
            "This is a test project",
            new List<string> { "Point 1", "Point 2" },
            new List<Skill> { new("Category", "Skill") },
            "Location",
            new DateOnly(2021, 1, 1),
            new DateOnly(2021, 1, 2));
    }
}
