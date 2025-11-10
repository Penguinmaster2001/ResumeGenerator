
namespace ProjectLogging.Cli;



public class CliAction
{
    public string Command { get; set; }
    public string SubCommand { get; set; }
    public string HelpText { get; set; }

    public Func<ParsedCliArguments, Task<ICliActionResult>> Action { get; set; }

    public List<CliArgument> ExpectedArguments { get; set; } = [];




    public CliAction(string command, string subCommand, string helpText, Func<ParsedCliArguments, Task<ICliActionResult>> action, List<CliArgument> expectedArguments)
    {
        Command = command;
        SubCommand = subCommand;
        HelpText = helpText;
        Action = action;
        ExpectedArguments = expectedArguments;
    }
}
