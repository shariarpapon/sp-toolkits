using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName =nameof(PlayerVerticalMovement), menuName = "SPToolkits/Motion Suppliers/" + nameof(PlayerVerticalMovement))]
    public class PlayerVerticalMovement : MotionSupplier
    {
        public float GravityScale { get; private set; } = 1;
        private float _scaledGravity = 0;
        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            MovementUtils.AppplyEdgeProximitySlip(ctx.controller, ctx.settings.motionCorrectionLayers, ctx.settings.edgeSlipSpeed, ctx.settings.edgeSlipThreshold, ctx.isCenterGrounded);

            _scaledGravity = ctx.settings.gravity * GravityScale;
            if (!ctx.isCenterGrounded)
                ctx.verticalVelocity += _scaledGravity * deltaTime * -ctx.LocalUp;
            else if (Vector3.Dot(ctx.verticalVelocity.normalized, ctx.LocalUp) <= 0f)
                ctx.verticalVelocity = -ctx.LocalUp;

            MovementUtils.CorrectCollisionProximityMotion(ctx.controller, ctx.settings.motionCorrectionLayers, ref ctx.verticalVelocity);
            ctx.controller.Move(ctx.verticalVelocity * deltaTime);
        }

        public void SetGravityScale(float scale) 
        {
            GravityScale = scale;
        }

    }
}