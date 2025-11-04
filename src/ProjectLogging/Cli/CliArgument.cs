
namespace ProjectLogging.Cli;



public record CliArgument(string Name,
                          string ShortName,
                          bool Required,
                          string Description,
                          Type ArgumentType,
                          Func<string, object?> ParseFunc,
                          object? DefaultValue)
{
    public static CliArgument Create<T>(string Name,
                          string ShortName,
                          bool Required,
                          string Description,
                          Func<string, T> ParseFunc,
                          T? DefaultValue)
    {
        return new(Name, ShortName, Required, Description, typeof(T), s => ParseFunc(s), DefaultValue);
    }
}
