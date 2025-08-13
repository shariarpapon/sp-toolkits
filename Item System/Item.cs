using UnityEngine;

namespace SPToolkits.ItemSystem
{
    [System.Serializable]
    public class Item : MonoBehaviour
    {
        public string itemName;
        public Sprite icon;
        public ItemContext itemContext;

        public bool IsActive { get { return gameObject.activeSelf; } }

        /// <summary>
        /// Prepares the item to be added to an item container.
        /// </summary>
        public virtual void PrepareForContainer()
        {

        }

        #region Helper Functions
        public void ResetTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void SetRotation(Vector3 eulerAngles) 
        {
            SetRotation(Quaternion.Euler(eulerAngles));
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetLocalRotation(Vector3 eulerAngles)
        {
            SetLocalRotation(Quaternion.Euler(eulerAngles));
        }

        public void SetLocalRotation(Quaternion rotation)
        {
            transform.localRotation = rotation;
        }

        public void SetForward(Vector3 direction) 
        {
            transform.forward = direction;
        }

        public void Activate() 
        {
            gameObject.SetActive(true);
        }
        public void Deactivate() 
        {
            gameObject.SetActive(false);
        }
        public void DestroyItem() 
        {
            Destroy(gameObject);
        }
        #endregion

        /// <param name="item">The item to use.</param>
        /// <param name="args">Any contextual data that needs to be passed in.</param>
        /// <returns>True, if the item was successfully used.</returns>
        public static bool Use(Item item, object args)
        {
            if (item == null)
            {
                Debug.LogWarning("Cannot use - item is null.");
                return false;
            }

            if (item.TryGetComponent(out IItemUseCallback useCallback))
                return useCallback.OnItemUse(args);
            return false;
        }
    }
}
