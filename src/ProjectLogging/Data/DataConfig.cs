
namespace ProjectLogging.Data;



public record DataConfig(
    DataEntry PersonalInfo,
    DataEntry Jobs,
    DataEntry Projects,
    DataEntry Skills,
    DataEntry Education,
    DataEntry Courses,
    DataEntry Volunteering,
    DataEntry Hobbies);



public record DataEntry(string Path, string Title = "", bool Include = false);
