using System.Collections.Generic;

namespace SPToolkits.ItemSystem
{
    public class ItemContainer
    {
        public readonly IItemContainerOwner owner;
        public event System.Action<ItemContainer> onItemSelectionChanged;
        public event System.Action<ItemContainer, Item> onItemAdded;
        public event System.Action<ItemContainer, Item> onItemRemoved;
        private List<Item> _items = new List<Item>();
        private int _selectionIndex = -1;
        private int SelectionIndex 
        {
            get { return _selectionIndex; }
            set 
            {
                if (_items.Count <= 0)
                {
                    _selectionIndex = -1;
                    return;
                }
                int _value = value;
                if (_value < 0) _value = _items.Count - 1;
                else if (_value > _items.Count - 1) _value = 0;
                _value = System.Math.Clamp(_value, 0, _items.Count - 1);

                if (_selectionIndex != _value)
                {
                    _selectionIndex = _value;
                    if (GetSelectedItem() != null) 
                        onItemSelectionChanged?.Invoke(this); 
                }
            }
        }

        /// <param name="owner">The owner of this item container.</param>
        /// <param name="capacity">The maximum number of items this container can hold.</param>
        public ItemContainer(IItemContainerOwner owner, uint capacity) 
        {
            this.owner = owner;
            _items = new List<Item>((int)capacity);
        }

        /// <param name="item">The item to add.</param>
        /// <param name="container">The destination ItemContainer.</param>
        /// <returns>True, if the item was successfully added.</returns>
        public static bool AddToContainer(ItemContainer container, Item item)
        {
            if (container.Add(item))
            {
                if (item.TryGetComponent(out IItemAddCallback addCallback))
                    addCallback.OnItemAddedToContainer(container);

                item.PrepareForContainer();

                return true;
            }
            return false;
        }

        public bool ContainsItem(Item item)
        {
            for (int i = 0; i < _items.Count; i++)
                if (_items[i].itemName == item.itemName)
                    return true;
            return false;
        }

        /// <summary>
        /// Increments/decrements selection index by delta.
        /// </summary>
        public void ScrollSelection(int delta) 
        {
            SetSelection(SelectionIndex + delta);
        }

        /// <summary>
        /// Swaps the items with the given indices.
        /// </summary>
        /// <param name="indexA">Index of the first item.</param>
        /// <param name="indexB">Index of the second item.</param>
        public void SwapItems(int indexA, int indexB) 
        {
            if (!IsValidIndex(indexA) || !IsValidIndex(indexB))
                return;
    
            //Apparently this tuple thing swaps the array elements
            (_items[indexB], _items[indexA]) = (_items[indexA], _items[indexB]);

            if (indexA == _selectionIndex || indexB == _selectionIndex)
                onItemSelectionChanged?.Invoke(this);
        }

        /// <summary>
        /// Swaps the indices of the given items, if they exist in the container.
        /// </summary>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        public void SwapItems(Item a, Item b) 
        {
            if (!_items.Contains(a) || !_items.Contains(b))
                return;
            SwapItems(_items.IndexOf(a), _items.IndexOf(b));
        }

        /// <summary>
        /// Set the current selection to the given item, if it exists in the inventory.
        /// </summary>
        /// <param name="item">The item to select.</param>
        public void SetSelection(Item item) 
        {
            SetSelection(GetIndex(item));
        }

        /// <summary>
        /// Set the current selected item index to the given index.
        /// </summary>
        /// <param name="index">The new selected item index.</param>
        public void SetSelection(int index)
        {
            SelectionIndex = index;
        }

        /// <param name="index">The index of the item.</param>
        /// <returns>
        /// The item at the given index. 
        /// <br>Returns null, if index is out of bounds.</br>
        /// </returns>
        public Item GetItemAt(int index)
        {
            if (index < 0 || index >= _items.Count)
                return null;
            return _items[index];
        }

        /// <summary>
        /// Make sure the selected item is valid and exists in the container.
        /// </summary>
        public void ValidateSelection() 
        {
            SelectionIndex = SelectionIndex;
        }

        /// <returns>The currently selected item.</returns>
        public Item GetSelectedItem() 
        {
            if (IsEmpty() || !IsValidIndex(SelectionIndex)) return null;
            return _items[SelectionIndex];
        }

        /// <summary>
        /// Removes the given item from the container.
        /// </summary>
        /// <param name="itemToRemove">The item to remove.</param>
        /// <param name="destroy">If true, the Item gameobject will be destroyed.</param>
        /// <returns>True, if the item was found and successfully removed.</returns>
        public bool RemoveItem(Item itemToRemove, bool destroy) 
        {
            foreach (Item item in _items)
                if (item == itemToRemove) 
                {
                    _items.Remove(itemToRemove);
                    onItemRemoved?.Invoke(this, item);
                    ValidateSelection();
                    if(destroy)
                        item.DestroyItem();
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Remove the currently selected item.
        /// </summary>
        /// <param name="destroy">If true, the Item gameobject will be destroyed.</param>
        public void RemoveSelectedItem(bool destroy)
        {
            Item selected = GetSelectedItem();
            RemoveItem(selected, destroy);
        }

        /// <returns>True, if the container holds no items.</returns>
        public bool IsEmpty() => _items.Count <= 0;

        /// <param name="item">The item get the index of.</param>
        /// <returns>
        /// The index of the item.
        /// <br>Returns -1, if the item does not exist in the container</br>.
        /// </returns>
        public int GetIndex(Item item) 
        {
            if (_items.Contains(item))
                return _items.IndexOf(item);

            return -1;
        }

        /// <param name="index">The index value to check.</param>
        /// <returns>True, of the index is within bounds.</returns>
        public bool IsValidIndex(int index) 
        {
            return index >= 0 && index < _items.Count;
        }

        /// <returns>Index of the currently selected item.</returns>
        public int GetSelectionIndex()
            => SelectionIndex;

        /// <returns>The maximum number of items the container can hold.</returns>
        public int GetCapacity()
            => _items.Capacity;

        /// <returns>The current number of items held in the container.</returns>
        public int GetItemCount()
            => _items.Count;

        /// <summary>
        /// Returns true if the item was succesfully added to the inventory.
        /// </summary>
        private bool Add(Item item)
        {
            if (_items.Contains(item) || item == null)
                return false;

            _items.Add(item);
            onItemAdded?.Invoke(this, item);
            if (GetSelectedItem() == null)
                SetSelection(_items.Count - 1);
            return true;
        }

    }
}
