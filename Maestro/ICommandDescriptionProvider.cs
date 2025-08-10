namespace SPToolkits.Maestro
{
    /// <summary>
    /// Provides description for the command.
    /// </summary>
    public interface ICommandDescriptionProvider 
    {
        /// <summary>
        /// Description to provide.
        /// </summary>
        string Description { get; }
    }
}