using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = nameof(PlayerRotation), menuName = "SPToolkits/Motion Suppliers/" + nameof(PlayerRotation))]
    public class PlayerRotation : MotionSupplier
    {
        public float turnSpeed = 20f;

        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            if(ctx.lateralVelocity.sqrMagnitude > Mathf.Epsilon)
                MovementUtils.SlerpRotateForward(ctx.transform, ctx.moveDirection, turnSpeed);
        }
    }
}