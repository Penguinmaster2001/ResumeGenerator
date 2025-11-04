
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
        var receivedParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var parsedArguments = new ParsedCliArguments();
        var parsedOptions = new ParsedCliArguments();

        string? commandString = null;
        string? subCommandString = null;

        List<(string Parameter, string? Value, bool ShortName)> parameterValues = [];

        for (int i = 1; i < args.Length; i++)
        {
            var arg = args[i];
            bool lastArg = i >= args.Length - 1;

            if (arg.StartsWith('-'))
            {
                bool shortName = !arg.StartsWith("--");

                var parameter = arg.TrimStart('-');

                if (!receivedParameters.Add(parameter))
                {
                    throw new Exception($"Already received parameter {parameter}");
                }

                string? value = null;
                if (!lastArg)
                {
                    var nextArg = args[i + 1];
                    if (!nextArg.StartsWith('-'))
                    {
                        if (commandString is not null)
                        {
                            if (subCommandString is not null)
                            {
                                value = nextArg;
                                i++;
                            }
                            else if (!_cliActions.ContainsCommand(commandString, nextArg))
                            {
                                value = nextArg;
                                i++;
                            }
                        }
                        else if (!_cliActions.ContainsCommand(nextArg))
                        {
                            value = nextArg;
                            i++;
                        }
                    }
                }
                parameterValues.Add((parameter, value, shortName));
            }
            else if (commandString is not null)
            {
                if (subCommandString is null && _cliActions.ContainsCommand(commandString, arg))
                {
                    subCommandString = arg;
                }
                else
                {
                    throw new Exception($"Unexpected argument {arg}");
                }
            }
            else if (_cliActions.ContainsCommand(arg))
            {
                commandString = arg;
            }
            else
            {
                throw new Exception($"Unexpected argument {arg}");
            }
        }

        if (commandString is null || subCommandString is null)
        {
            throw new Exception("Command not found");
        }

        var action = _cliActions.GetAction(commandString, subCommandString);

        var expectedArguments = new ExpectedArguments(action.ExpectedArguments);

        foreach (var (parameter, value, shortName) in parameterValues)
        {
            if (expectedArguments.AddArgument(shortName, parameter, !string.IsNullOrWhiteSpace(value)))
            {
                receivedParameters
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
    public ParsedCliArguments Arguments { get; set; }
    public ParsedCliArguments Options { get; set; }
    public CliAction Action { get; set; }




    public CliParseResults(ParsedCliArguments arguments, ParsedCliArguments options, CliAction action)
    {
        Arguments = arguments;
        Options = options;
        Action = action;
    }
}
