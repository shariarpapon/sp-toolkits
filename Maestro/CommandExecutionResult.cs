namespace SPToolkits.Maestro
{
    public sealed class CommandExecutionResult 
    {
        public readonly CommandExecutionStatus executionStatus;
        public readonly IMaestroCommand commandDefinition;
        public readonly ParsedCommand parsedCommand;
        public readonly System.Exception exception = null;

        public CommandExecutionResult(CommandExecutionStatus executionStatus, ParsedCommand parsedCommand, IMaestroCommand commandDefinition) 
        {
            this.executionStatus = executionStatus;
            this.parsedCommand = parsedCommand;
            this.commandDefinition = commandDefinition;
        }

        public CommandExecutionResult(CommandExecutionStatus executionStatus, ParsedCommand parsedCommand, IMaestroCommand commandDefinition, System.Exception exception)
        {
            this.executionStatus = executionStatus;
            this.parsedCommand = parsedCommand;
            this.commandDefinition = commandDefinition;
            this.exception = exception;
        }
    }
}