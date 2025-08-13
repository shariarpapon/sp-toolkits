namespace SPToolkits.ItemSystem
{
    /// <summary>
    /// Any item can implement this to add item use functionalities.
    /// </summary>
    public interface IItemUseCallback
    {
        ///<param name="argument">Any relevant data that needs to be passed in.</param>
        /// <returns>True, if item was successfully used.</returns>
        public bool OnItemUse(object argument);
    }
}