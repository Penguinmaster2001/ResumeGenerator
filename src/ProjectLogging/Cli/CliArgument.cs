
namespace ProjectLogging.Cli;



public class CliArgument
{
    public string Name { get; }
    public string ShortName { get; }
    public bool Required { get; }
    public string Description { get; }
    public Type ArgumentType { get; }
    public Func<string, object?> ParseFunc { get; }
    public object? DefaultValue { get; }



    private CliArgument(
        string name,
        string shortName,
        bool required,
        string description,
        Type argumentType,
        Func<string, object?> parseFunc,
        object? defaultValue)
    {
        Name = name;
        ShortName = shortName;
        Required = required;
        Description = description;
        ArgumentType = argumentType;
        ParseFunc = parseFunc;
        DefaultValue = defaultValue;
    }



    public static CliArgument Create<T>(
        string name,
        string shortName,
        bool required,
        string description,
        Func<string, T> parseFunc,
        T? defaultValue)
    {
        return new(name, shortName, required, description, typeof(T), s => parseFunc(s), defaultValue);
    }
}
