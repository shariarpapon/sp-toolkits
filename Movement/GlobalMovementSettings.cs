using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName ="Global Movement Settings", menuName ="Wiz/Global Movement Settings", order = 0)]
    public sealed class GlobalMovementSettings : ScriptableObject
    {
        [Tooltip("Layers from which the player can jump")]
        public LayerMask whatsIsGround;

        [Tooltip("Colliding with slopes with these layers will correct the collision motion.")]
        public LayerMask motionCorrectionLayers;

        public float gravity = 22;

        [Tooltip("If the angle between the local Up vector and the ground normal is greater than this value, the controller will be considered NOT-grounded.")]
        public float maxStableSlopeAngle = 30.0f;

        public float edgeSlipSpeed = 5.0f;
        [Range(0, 1)]
        [Tooltip("Controls how far excess-mass relateive to the controller center can go off an edge before slipping. " +
                 "1 means far as possible and 0 means as little possible before slipping.")]
        public float edgeSlipThreshold = 0.0f;
    }
}