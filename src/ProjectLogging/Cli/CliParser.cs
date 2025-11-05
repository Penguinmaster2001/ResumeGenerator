
using System.Diagnostics.CodeAnalysis;



namespace ProjectLogging.Cli;



public class CliParser
{
    private readonly CliActionCollection _cliActions;
    private readonly ExpectedArguments _expectedOptions;



    public CliParser(CliActionCollection cliActions, IEnumerable<CliArgument> expectedOptions)
    {
        _cliActions = cliActions;
        _expectedOptions = new(expectedOptions);
    }



    public CliParseResults ParseArgs(string[] args)
    {
        if (args.Length < 3)
        {
            throw new Exception("Not enough arguments.");
        }

        if (!_cliActions.TryGetAction(args[1], args[2], out var action))
        {
            throw new Exception($"Unknown command: {args[1]} {args[2]}.");
        }

        var expectedParameters = new ExpectedArguments(action.ExpectedArguments);

        for (int i = 3; i < args.Length; i++)
        {
            if (!args[i].StartsWith('-'))
            {
                throw new Exception($"Unexpected argument {args[i]}");
            }

            ExpectedArguments currentArgList;
            CliArgument argument;
            if (expectedParameters.TryGetUnsatisfied(args[i], out argument))
            {
                currentArgList = expectedParameters;
            }
            else if (_expectedOptions.TryGetUnsatisfied(args[i], out argument))
            {
                currentArgList = _expectedOptions;
            }
            else
            {
                throw new Exception($"Could not find argument {args[i]}");
            }

            if (i + 1 >= args.Length || args[i + 1].StartsWith('-'))
            {
                if (argument.DefaultValue is not null)
                {
                    throw new Exception($"Expected value for {args[i]}");
                }

                currentArgList.AddArgument(args[i], false);
            }
        }

        return new(parsedArguments, parsedOptions, action);
    }



    private class CliArgumentComparer : IEqualityComparer<CliArgument>
    {
        public bool Equals(CliArgument? x, CliArgument? y)
        {
            if (x is null) return y is null;

            if (y is null) return false;

            return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
                || string.Equals(x.ShortName, y.ShortName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] CliArgument obj)
        {
            return HashCode.Combine(obj.Name, obj.ShortName);
        }
    }
}



public class CliParseResults
{
    public required ParsedCliArguments Arguments { get; init; }
    public required ParsedCliArguments Options { get; init; }
    public required CliAction Action { get; init; }




    public static CliParseResults Success(ParsedCliArguments arguments, ParsedCliArguments options, CliAction action)
    {
        return new CliParseResults
        {
            Arguments = arguments,
            Options = options,
            Action = action,
        };
    }
}
