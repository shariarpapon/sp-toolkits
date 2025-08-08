using UnityEngine;

namespace SPToolkits.Movement
{
    public sealed class HitInfo
    {
        public readonly Vector3 impact;
        public readonly Vector3 origin;
        public readonly Vector3 direction;
        public readonly Vector3 normal;

        public HitInfo(Vector3 impact, Vector3 origin, Vector3 direction, Vector3 normal) 
        {
            this.impact = impact;
            this.origin = origin;
            this.direction = direction;
            this.normal = normal;
        }

        public float GetSqrDistance()
            => (impact - origin).sqrMagnitude;
    }
}