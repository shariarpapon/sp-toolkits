using UnityEngine;

namespace SPToolkits.TweeningUtils
{
    public sealed class PositionCopier : MonoBehaviour
    {
        [System.Serializable]
        private enum UpdateMode 
        {
            NormalUpdate,
            LateUpdate,
            FixedUpdate
        }

        [SerializeField]
        private UpdateMode updateMode;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector3 offset;

        private void Awake()
        {
            transform.position = target.position;
        }

        private void Update()
        {
            if (updateMode == UpdateMode.NormalUpdate)
                transform.position = target.position + offset;
        }

        private void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate)
                transform.position = target.position + offset;
        }

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate)
                transform.position = target.position + offset;
        }
    }
}
