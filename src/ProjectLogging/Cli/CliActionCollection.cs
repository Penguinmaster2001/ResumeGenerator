
using System.Collections;
using System.Diagnostics.CodeAnalysis;



namespace ProjectLogging.Cli;



public class CliActionCollection : ICollection<CliAction>
{
    private readonly Dictionary<string, Dictionary<string, CliAction>> _actions = new(StringComparer.OrdinalIgnoreCase);

    public int Count { get; private set; }
    public bool IsReadOnly => false;



    public CliAction GetAction(string command, string subCommand)
    {
        if (!_actions.TryGetValue(command, out var subCommands))
        {
            throw new ArgumentException($"Could not find command {command}.", nameof(command));
        }

        if (!subCommands.TryGetValue(subCommand, out var action))
        {
            throw new ArgumentException($"Could not find subcommand {subCommand} of command {command}.", nameof(subCommand));
        }

        return action;
    }



    public bool TryGetAction(string command, string subCommand, [NotNullWhen(true)] out CliAction? action)
    {
        if (!_actions.TryGetValue(command, out var subCommands))
        {
            action = default;
            return false;
        }

        if (!subCommands.TryGetValue(subCommand, out action))
        {
            return false;
        }

        return true;
    }



    public void Add(CliAction action)
    {
        if (!_actions.TryGetValue(action.Command, out var subCommands))
        {
            subCommands = new(StringComparer.OrdinalIgnoreCase);
            _actions.Add(action.Command, subCommands);
        }

        if (!subCommands.ContainsKey(action.SubCommand))
        {
            Count++;
        }

        subCommands.Add(action.SubCommand, action);
    }



    public void Clear()
    {
        Count = 0;
        _actions.Clear();
    }



    public bool Contains(CliAction action)
    {
        return _actions.TryGetValue(action.Command, out var subCommands) && subCommands.ContainsKey(action.SubCommand);
    }



    public bool ContainsCommand(string command)
    {
        return _actions.ContainsKey(command);
    }



    public bool ContainsCommand(string command, string subCommand)
    {
        return _actions.TryGetValue(command, out var subCommands) && subCommands.ContainsKey(subCommand);
    }



    public void CopyTo(CliAction[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

        if (Count > array.Length - arrayIndex)
        {
            throw new ArgumentException("The destination array has fewer elements than the collection.");
        }

        var enumerator = GetEnumerator();

        for (int i = 0; i < Count; i++)
        {
            array[i + arrayIndex] = enumerator.Current;
            enumerator.MoveNext();
        }
    }



    public bool Remove(CliAction action)
    {
        if (!_actions.TryGetValue(action.Command, out var subCommands))
        {
            return false;
        }

        return subCommands.Remove(action.SubCommand);
    }



    public IEnumerator<CliAction> GetEnumerator()
    {
        foreach (var subCommands in _actions.Values)
        {
            foreach (var action in subCommands.Values)
            {
                yield return action;
            }
        }
    }



    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
