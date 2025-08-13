using UnityEngine;

namespace SPToolkits.ItemSystem.UI
{
    public sealed class ItemContainerUI : MonoBehaviour
    {
        [SerializeField]
        private ItemSlotUI[] slots;

        public void Initialize(ItemContainer itemContainer) 
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Initialize(itemContainer, i);
            }
        }

    }
}