namespace SPToolkits.Maestro
{
    public interface IMaestroIOHandler
    {
        /// <summary>
        /// Writes the output string to the terminal's output display.
        /// </summary>
        /// <param name="output">The string to output.</param>
        void Write(string output);

        /// <summary>
        /// Reads the input from the terminal's input field.
        /// </summary>
        /// <returns>The read input as string.</returns>
        string Read();

        /// <summary>
        /// Clears the terminal's input field.
        /// </summary>
        void ClearInput();
    }
}
