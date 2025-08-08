using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName ="Player Vertical Movement", menuName =$"SPToolkits/Motion Suppliers/Player Vertical Movement")]
    public class PlayerVerticalMovement : MotionSupplier
    {
        public float gravity = 22;

        protected override void Tick(float deltaTime, MotionControlContext ctx)
        {
            if (!ctx.isCenterGrounded)
                ctx.verticalVelocity += deltaTime * gravity * -ctx.transform.up;
            else if (Vector3.Dot(ctx.verticalVelocity.normalized, ctx.transform.up) <= 0f)
                ctx.verticalVelocity = -ctx.transform.up;

            MovementUtils.CorrectCollisionProximityMotion(ctx.controller, ctx.settings.motionCorrectionLayers, ref ctx.verticalVelocity);
            ctx.controller.Move(ctx.verticalVelocity * deltaTime);
        }

    }
}