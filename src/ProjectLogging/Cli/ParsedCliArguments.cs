
using System.Diagnostics.CodeAnalysis;



namespace ProjectLogging.Cli;



public class ParsedCliArguments
{
    private readonly List<object?> _parsedArgDatabase = [];
    private readonly Dictionary<string, int> _parsedArgumentIds = new(StringComparer.OrdinalIgnoreCase);



    public T GetArgument<T>(string argumentName)
    {
        if (!_parsedArgumentIds.TryGetValue(argumentName, out var parsedArgumentId))
        {
            throw new ArgumentException($"Unable to find argument {argumentName}", nameof(argumentName));
        }

        var parsedArgument = _parsedArgDatabase[parsedArgumentId];

        if (parsedArgument is not T typedArgument)
        {
            throw new ArgumentException($"Argument {argumentName} is not assignable to {typeof(T).Name}",
                nameof(argumentName));
        }

        return typedArgument;
    }



    public bool TryGetArgument<T>(string argumentName, [NotNullWhen(true)] out T? value)
    {
        if (!_parsedArgumentIds.TryGetValue(argumentName, out var parsedArgumentId))
        {
            value = default;
            return false;
        }

        var parsedArgument = _parsedArgDatabase[parsedArgumentId];

        if (parsedArgument is not T typedArgument)
        {
            value = default;
            return false;
        }

        value = typedArgument;
        return true;
    }



    public void SetArgument(CliArgument argument, string value)
    {
        try
        {
            var parsed = argument.ParseFunc(value);

            var id = _parsedArgDatabase.Count;
            _parsedArgDatabase.Add(parsed);

            _parsedArgumentIds.Add(argument.Name, id);
            _parsedArgumentIds.Add(argument.ShortName, id);
        }
        catch (CliArgumentParseException)
        {
            throw;
        }
    }
}
