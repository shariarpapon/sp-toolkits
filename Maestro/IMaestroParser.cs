namespace SPToolkits.Maestro 
{
    /// <summary>
    /// Implement this interface to define a parser that can be used by the maestro terminal.
    /// </summary>
    public interface IMaestroParser 
    {
        ParserOutput Parse(string source);
    }
}