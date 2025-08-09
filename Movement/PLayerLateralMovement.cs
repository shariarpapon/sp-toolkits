using UnityEngine;
using SPToolkits.InputSystem;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = "Basic Player Movement", menuName = "Wiz/Motion Suppliers/Basic Player Movement")]
    public class PLayerLateralMovement : MotionSupplier
    {
        public float walkSpeed = 4f;
        public float runSpeed = 6f;

        protected override void _Tick(float deltaTime, MotionControlContext ctx)
        {
            float moveSpeed = InputManager.InputProvider.Sprint ? runSpeed : walkSpeed;

            float x = InputManager.InputProvider.RawInputX;
            float z = InputManager.InputProvider.RawInputY;
            ctx.moveDirection =  new Vector3(x, 0.0f, z).normalized;
            ctx.moveDirection = MovementUtils.ToLocalDirectionWithProjectedForward(ctx.transform, ctx.viewCamera.transform.forward, ctx.moveDirection);

            ctx.lateralVelocity = ctx.moveDirection * moveSpeed * deltaTime;
            MovementUtils.CorrectCollisionProximityMotion(ctx.controller, ctx.settings.motionCorrectionLayers, ref ctx.lateralVelocity, 30);

            ctx.controller.Move(ctx.lateralVelocity);
        }
    }
}
