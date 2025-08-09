using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = "Player Rotation", menuName = "Wiz/Motion Suppliers/Player Rotation")]
    public class PlayerRotation : MotionSupplier
    {
        public float turnSpeed = 20f;

        protected override void _Tick(float deltaTime, MotionControlContext ctx)
        {
            if(ctx.lateralVelocity.sqrMagnitude > Mathf.Epsilon)
                MovementUtils.SlerpRotateForward(ctx.transform, ctx.moveDirection, turnSpeed);
        }
    }
}