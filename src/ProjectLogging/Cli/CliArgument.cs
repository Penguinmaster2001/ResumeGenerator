
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
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(shortName, nameof(shortName));
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        Name = name;
        ShortName = shortName;
        Required = required;
        Description = description;
        ArgumentType = argumentType ?? throw new ArgumentNullException(nameof(argumentType));
        ParseFunc = parseFunc ?? throw new ArgumentNullException(nameof(parseFunc));
        DefaultValue = defaultValue;
    }



    public static CliArgument Create<T>(
        string name,
        string shortName,
        bool required,
        string description,
        T? defaultValue)
        where T : IParsable<T>
        => Create(
            name,
            shortName,
            required,
            description,
            s => T.TryParse(s, null, out var result) ? result : defaultValue,
            defaultValue);



    public static CliArgument Create<T>(
        string name,
        string shortName,
        bool required,
        string description,
        Func<string, T> parseFunc,
        T? defaultValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(shortName, nameof(shortName));
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        return new(name, shortName, required, description, typeof(T), s => parseFunc(s), defaultValue);
    }
}
