
namespace ProjectLogging.Cli;



public class ExpectedArguments
{
    private readonly HashSet<CliArgument> _satisfiedArguments = [];
    private readonly List<CliArgument> _expectedArguments = [];
    private readonly Dictionary<string, int> _expectedArgumentNameIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _expectedArgumentShortNameIds = new(StringComparer.OrdinalIgnoreCase);



    public ExpectedArguments(IEnumerable<CliArgument> expectedArguments)
    {
        foreach (var argument in expectedArguments)
        {
            int id = _expectedArguments.Count;
            _expectedArguments.Add(argument);
            _expectedArgumentNameIds.Add(argument.Name, id);
            _expectedArgumentShortNameIds.Add(argument.ShortName, id);
        }
    }



    public bool FullySatisfied => _satisfiedArguments.Count == _expectedArguments.Count;



    public bool AddArgument(bool shortName, string name, bool withValue)
    {
        int id;
        if (shortName)
        {
            if (!_expectedArgumentShortNameIds.TryGetValue(name, out id)) return false;
        }
        else
        {
            if (!_expectedArgumentNameIds.TryGetValue(name, out id)) return false;
        }

        if (!withValue && _expectedArguments[id].DefaultValue is null) return false;

        return _satisfiedArguments.Add(_expectedArguments[id]);
    }
}
