using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName ="Player Vertical Movement", menuName ="Wiz/Motion Suppliers/Player Vertical Movement")]
    public class PlayerVerticalMovement : MotionSupplier
    {
        protected override void _Tick(float deltaTime, MotionControlContext ctx)
        {
            MovementUtils.AppplyEdgeProximitySlipToController(ctx.controller, ctx.settings.motionCorrectionLayers, ctx.settings.edgeSlipSpeed, ctx.settings.edgeSlipThreshold, ctx.isCenterGrounded);

            if (!ctx.isCenterGrounded)
                ctx.verticalVelocity += deltaTime * ctx.settings.gravity * -ctx.LocalUp;
            else if (Vector3.Dot(ctx.verticalVelocity.normalized, ctx.LocalUp) <= 0f)
                ctx.verticalVelocity = -ctx.LocalUp;

            MovementUtils.CorrectCollisionProximityMotion(ctx.controller, ctx.settings.motionCorrectionLayers, ref ctx.verticalVelocity);
            ctx.controller.Move(ctx.verticalVelocity * deltaTime);
        }

    }
}