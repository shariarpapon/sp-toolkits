using System.Collections.Generic;

namespace SPToolkits.Maestro
{
    public sealed class CommandHandler
    {
        private readonly Dictionary<string, IMaestroCommand> _commands;
        private readonly MaestroTerminal _terminal;

        /// <summary>
        /// Handles parsing and executing the commands.
        /// </summary>
        /// <param name="commands">The command definitons for this terminal.</param>
        public CommandHandler(MaestroTerminal terminal, IEnumerable<IMaestroCommand> commands) 
        {
            _terminal = terminal;
            _commands = new Dictionary<string, IMaestroCommand>();
            foreach (IMaestroCommand cmd in commands)
            {
                if (_commands.ContainsKey(cmd.Keyword))
                    throw new System.Exception($"More than one command with the keyword <{cmd.Keyword}>");
                _commands.Add(cmd.Keyword, cmd);
            }
        }

        public CommandExecutionResult Execute(ParsedCommand parsedCommand) 
        {
            try
            {
                IMaestroCommand command = null;
                if (string.IsNullOrEmpty(parsedCommand.keyword))
                {
                    return new CommandExecutionResult(CommandExecutionStatus.KeywordNullOrEmpty, parsedCommand, null);
                }
                else if (_commands.ContainsKey(parsedCommand.keyword))
                {
                    command = _commands[parsedCommand.keyword];
                    if (parsedCommand.argumentCount < command.MinArgCount)
                        return new CommandExecutionResult(CommandExecutionStatus.InvalidArgumentCount, parsedCommand, command);
                    else if (command.Execute(_terminal, parsedCommand.arguments))
                        return new CommandExecutionResult(CommandExecutionStatus.Successful, parsedCommand, command);
                    else
                        return new CommandExecutionResult(CommandExecutionStatus.FailedExecution, parsedCommand, command);
                }
                else
                {
                    return new CommandExecutionResult(CommandExecutionStatus.KeywordNotFound, parsedCommand, null);
                }
            }
            catch (System.Exception exception)
            {
                return new CommandExecutionResult(CommandExecutionStatus.FatalError, parsedCommand, null, exception);
            }
        }

        public CommandExecutionResult[] Execute(ParsedCommand[] parsedCommand)
        {
            CommandExecutionResult[] results = new CommandExecutionResult[parsedCommand.Length];
            for (int i = 0; i < parsedCommand.Length; i++)
                results[i] = Execute(parsedCommand[i]);
            return results;
        }
    }
}
