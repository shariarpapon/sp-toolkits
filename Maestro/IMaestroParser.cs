namespace SPToolkits.Maestro 
{
    /// <summary>
    /// Implement this interface to define a parser that can be used by the maestro terminal.
    /// </summary>
    public interface IMaestroParser 
    {
        public char Escape { get; set; }
        public char CommandStart { get; set; }
        public char StringLiteral { get; set; }
        ParserOutput Parse(string source);
    }
}