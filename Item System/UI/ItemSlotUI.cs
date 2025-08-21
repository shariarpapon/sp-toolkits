using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SPToolkits.Extensions;

namespace SPToolkits.ItemSystem.UI
{
    public sealed class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static ItemSlotUI _CurrentDraggingSlot = null;
        private static ItemSlotUI _HoveredSlot = null;

        private Image _iconImage;
        private int _itemIndex;
        private Vector3 _originalPosition;
        private Vector3 _mouseOffset;
        private ItemContainer _itemContainer;

        private void OnEnable()
        {
            UpdateSlotUI();
        }

        private void OnDisable()
        {
            UpdateSlotUI();
        }

        public void Initialize(ItemContainer itemContainer, int itemIndex) 
        {
            _iconImage = GetComponent<Image>();
            _itemIndex = itemIndex;
            _itemContainer = itemContainer;
            _itemContainer.onItemAdded += OnItemContainerModified;
            _itemContainer.onItemRemoved += OnItemContainerModified;
            UpdateSlotUI();
        }

        private void OnDestroy()
        {
            _itemContainer.onItemAdded -= OnItemContainerModified;
            _itemContainer.onItemRemoved -= OnItemContainerModified;
        }
        
        private void OnItemContainerModified(ItemContainer container, Item modifiedItem) 
        {
            UpdateSlotUI();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_CurrentDraggingSlot != null)
                return;
            _iconImage.transform.SetAsLastSibling();
            _CurrentDraggingSlot = this;
            _iconImage.raycastTarget = false;
            _originalPosition = _iconImage.transform.localPosition;
            _mouseOffset = _iconImage.transform.localPosition - Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_CurrentDraggingSlot != this)
                return;
            _iconImage.transform.localPosition = Input.mousePosition + _mouseOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_CurrentDraggingSlot != this)
                return;
            _iconImage.raycastTarget = true;
            _CurrentDraggingSlot = null;
            _iconImage.transform.localPosition = _originalPosition;

            //Swap items
            if (_HoveredSlot != this && _HoveredSlot != null) 
            {
                _itemContainer.SwapItems(_itemIndex, _HoveredSlot._itemIndex);
                UpdateSlotUI();
                _HoveredSlot.UpdateSlotUI();
            }
            _HoveredSlot = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _HoveredSlot = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_HoveredSlot == this)
                _HoveredSlot = null;
        }

        public void UpdateSlotUI() 
        {
            if (_itemContainer == null)
                return;
            Item item = _itemContainer.GetItemAt(_itemIndex);
            if (item == null)
            {
                _iconImage.MakeTransparent();
                return;
            }
            _iconImage.sprite = item.icon;
            _iconImage.MakeOpaque();
        }

    }
}