namespace SPToolkits.Maestro
{
    /// <summary>
    /// Implement this interface to define a command that can be executed through a meastro terminal.
    /// </summary>
    public interface IMaestroCommand
    {
        /// <summary>
        /// The keyword for this command.
        /// <br><b>IMPORTANT</b>Cannot have more then one of the same keyword.</br>
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// The minimum required number of arguments.
        /// </summary>
        uint MinimumArgumentCount { get; }

        /// <summary>
        /// Called upon entering the command in the terminal.
        /// </summary>
        /// <param name="terminal">The terminal which initiated the command executaion.</param>
        /// <param name="args">Arguments of the command</param>
        /// <returns>True if executed succesfully.</returns>
        bool Execute(MaestroTerminal terminal, string[] args);
    }
}
