namespace SPToolkits.ItemSystem
{
    public interface IItemAddCallback
    {
        /// <summary>
        /// Called when item is added to an ItemContainer.
        /// </summary>
        /// <param name="container">The ItemContainer the item is added to.</param>
        public void OnItemAddedToContainer(ItemContainer container);        
    }
}