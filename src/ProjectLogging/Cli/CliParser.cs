
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

        // Get command and subcommand
        if (!_cliActions.TryGetAction(args[1], args[2], out var action))
        {
            throw new Exception($"Unknown command: {args[1]} {args[2]}.");
        }

        var expectedParameters = new ExpectedArguments(action.ExpectedArguments);
        var parsedParameters = new ParsedCliArguments();
        var parsedOptions = new ParsedCliArguments();

        // Get parameters and options
        for (int i = 3; i < args.Length; i++)
        {
            if (!args[i].StartsWith('-'))
            {
                throw new Exception($"Unexpected argument {args[i]}");
            }

            // Check for parameter or option
            ExpectedArguments currentArgList;
            ParsedCliArguments currentCliArgs;
            if (expectedParameters.TryGetUnsatisfied(args[i], out var argument))
            {
                currentArgList = expectedParameters;
                currentCliArgs = parsedParameters;
            }
            else if (_expectedOptions.TryGetUnsatisfied(args[i], out argument))
            {
                currentArgList = _expectedOptions;
                currentCliArgs = parsedOptions;
            }
            else
            {
                throw new Exception($"Could not find argument {args[i]}");
            }

            // Check for default value or get next value
            if (i + 1 >= args.Length || args[i + 1].StartsWith('-'))
            {
                if (argument.DefaultValue is not null)
                {
                    throw new Exception($"Expected value for {args[i]}");
                }

                currentArgList.AddArgument(args[i], false);
                currentCliArgs.SetArgument(argument);
            }
            else
            {
                currentArgList.AddArgument(args[i], true);
                currentCliArgs.SetArgument(argument, args[i + 1]);
                i++;
            }
        }

        if (!_expectedOptions.FullySatisfied || !expectedParameters.FullySatisfied)
        {
            throw new Exception("Unsatisfied arguments.");
        }

        return new(parsedParameters, parsedOptions, action);
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
    public ParsedCliArguments Arguments { get; init; }
    public ParsedCliArguments Options { get; init; }
    public CliAction Action { get; init; }




    public CliParseResults(ParsedCliArguments arguments, ParsedCliArguments options, CliAction action)
    {
        Arguments = arguments;
        Options = options;
        Action = action;
    }
}
