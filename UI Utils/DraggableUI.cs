using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SPToolkits.UIUtils
{
    public class DraggableUI : Image, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static DraggableUI DragTarget { get; private set; }

        private Vector2 offset;
        private Vector2 position
        {
            get { return transform.localPosition; }
            set { transform.localPosition = value; }
        }
        private Vector2 size
        {
            get { return ((RectTransform)transform).rect.size; }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragTarget = this;
            offset = position - eventData.position;
            transform.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragTarget == this)
            {
                FitWithinFrame();
                DragTarget = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsWithinFrame())
            {
                position = eventData.position + offset;
                FitWithinFrame();
            }
        }

        private bool IsWithinFrame()
        {
            Vector2 xRange = new Vector2(-Screen.width / 2f + size.x / 2f, Screen.width / 2f - size.x / 2f);
            Vector2 yRange = new Vector2(-Screen.height / 2f + size.y / 2f, Screen.height / 2f - size.y / 2f);

            bool inXRange = position.x <= xRange.y && position.x >= xRange.x;
            bool inYRange = position.y <= yRange.y && position.y >= yRange.x;

            return inXRange && inYRange;
        }

        private void FitWithinFrame()
        {
            Vector2 xRange = new Vector2(-Screen.width / 2f + size.x / 2f, Screen.width / 2f - size.x / 2f);
            Vector2 yRange = new Vector2(-Screen.height / 2f + size.y / 2f, Screen.height / 2f - size.y / 2f);
            Vector2 pos = position;

            pos.x = Mathf.Clamp(pos.x, xRange.x, xRange.y);
            pos.y = Mathf.Clamp(pos.y, yRange.x, yRange.y);
            position = pos;

        }
    }
}