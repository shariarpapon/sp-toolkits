using UnityEngine;

namespace SPToolkits.TweeningUtils
{
    public sealed class RotationCopier : MonoBehaviour
    {
        public Transform target;
        public void Update()
        {
            transform.rotation = target.rotation;
        }
    }
}