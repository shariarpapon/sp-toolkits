using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName ="Global Movement Settings", menuName ="Wiz/Global Movement Settings", order = 0)]
    public sealed class GlobalMovementSettings : ScriptableObject
    {
        [Header("Forward Movement")]
        public float walkSpeed = 4.0f;
        public float runSpeed = 6.0f;
        public float gravity = 22;
        public float maxStableSlopeAngle = 30.0f;

        [Header("Jumping")]
        public float maxJumpHeight = 1.5f;
        public float jumpInputBuffer = 0.15f;
        public float coyoteTime = 0.15f;

        [Header("Turning")]
        public float turnSpeed = 20.0f;
        public float aimTurnSpeed = 20.0f;

        [Header("Dashing")]
        public float dashDistance = 4.0f;
        public float dashSpeed = 30.0f;

        [Header("Other")]
        [Tooltip("Layers from which the player can jump")]
        public LayerMask groundLayers;
        [Tooltip("Colliding with slopes with these layers will correct the collision motion.")]
        public LayerMask motionCorrectionLayers;
        public float edgeSlipSpeed = 5.0f;
        [Range(0.0f, 1.0f), Tooltip("How much the center-of-mass can go off the edge before slipping.")]
        public float edgeSlipThreshold = 0.0f;
    }
}