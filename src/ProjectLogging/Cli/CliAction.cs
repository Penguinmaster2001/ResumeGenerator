
namespace ProjectLogging.Cli;



public class CliAction
{
    public string Command { get; set; }
    public string SubCommand { get; set; }
    public string HelpText { get; set; }

    public Func<CliParseResults, Task<ICliActionResult>> Action { get; set; }

    public List<CliArgument> ExpectedArguments { get; set; } = [];




    public CliAction(
        string command,
        string subCommand,
        string helpText,
        Func<CliParseResults, Task<ICliActionResult>> action,
        List<CliArgument> expectedArguments)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command, nameof(command));
        ArgumentException.ThrowIfNullOrWhiteSpace(subCommand, nameof(subCommand));
        ArgumentException.ThrowIfNullOrWhiteSpace(helpText, nameof(helpText));

        Command = command;
        SubCommand = subCommand;
        HelpText = helpText;
        Action = action ?? throw new ArgumentNullException(nameof(action));
        ExpectedArguments = expectedArguments ?? throw new ArgumentNullException(nameof(expectedArguments));
    }
}
