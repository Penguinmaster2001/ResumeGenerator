
using System.Diagnostics.CodeAnalysis;

namespace ProjectLogging.Cli;



public class ExpectedArguments
{
    private readonly HashSet<int> _satisfiedArguments = [];
    private readonly List<(CliArgument Arg, int Id)> _expectedArguments = [];
    private readonly Dictionary<string, int> _expectedArgumentNameIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _expectedArgumentShortNameIds = new(StringComparer.OrdinalIgnoreCase);



    public ExpectedArguments(IEnumerable<CliArgument> expectedArguments)
    {
        foreach (var argument in expectedArguments)
        {
            int id = _expectedArguments.Count;
            _expectedArguments.Add((argument, id));
            _expectedArgumentNameIds.Add(argument.Name, id);
            _expectedArgumentShortNameIds.Add(argument.ShortName, id);
        }
    }



    public bool FullySatisfied => _expectedArguments.All(a => !a.Arg.Required || _satisfiedArguments.Contains(a.Id));



    public bool Contains(string argument)
    {
        if (!argument.StartsWith('-')) return false;

        if (argument.StartsWith("--"))
        {
            return _expectedArgumentNameIds.ContainsKey(argument[2..]);
        }
        else
        {
            return _expectedArgumentShortNameIds.ContainsKey(argument[1..]);
        }
    }



    public bool TryGetUnsatisfied(string argument, [NotNullWhen(true)] out CliArgument? foundArg)
    {
        if (!argument.StartsWith('-'))
        {
            foundArg = null;
            return false;
        }

        int id;
        if (argument.StartsWith("--"))
        {
            if (!_expectedArgumentNameIds.TryGetValue(argument[2..], out id))
            {
                foundArg = null;
                return false;
            }
        }
        else
        {
            if (!_expectedArgumentShortNameIds.TryGetValue(argument[1..], out id))
            {
                foundArg = null;
                return false;
            }
        }

        if (_satisfiedArguments.Contains(id))
        {
            foundArg = null;
            return false;
        }

        foundArg = _expectedArguments[id].Arg;
        return true;
    }



    public bool AddArgument(string argument, bool withValue)
    {
        if (!argument.StartsWith('-')) return false;

        int id;
        if (argument.StartsWith("--"))
        {
            if (!_expectedArgumentNameIds.TryGetValue(argument[2..], out id)) return false;
        }
        else
        {
            if (!_expectedArgumentShortNameIds.TryGetValue(argument[1..], out id)) return false;
        }

        if (!withValue && _expectedArguments[id].Arg.DefaultValue is null) return false;

        return _satisfiedArguments.Add(id);
    }
}
