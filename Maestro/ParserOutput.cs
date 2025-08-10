namespace SPToolkits.Maestro
{
    /// <summary>
    /// Holds the output data of a parsed source.
    /// </summary>
    public sealed class ParserOutput
    {
        public readonly ParseStatus status;
        public readonly ParsedCommand[] commands;

        public ParserOutput(ParseStatus status, ParsedCommand[] commands) 
        {
            this.status = status;
            this.commands = commands;
        }
    }
}
