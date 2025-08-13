//#define BOB_LOGIC

using UnityEngine;
using SPToolkits.InputSystem;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = "Basic Player Movement", menuName = "Wiz/Motion Suppliers/Basic Player Movement")]
    public class PlayerLateralMovement : MotionSupplier
    {
        public float walkSpeed = 4f;
        public float runSpeed = 6f;
        public float NormalizedSpeed { get; private set; }
        public RuntimeControlContext RuntimeCtx { get; private set; }

        private float _activeMoveSpeed;

#if BOB_LOGIC
        public float bobHeight = 3f;
        public float bobFreq = 1f;
#endif

        public override void Init(RuntimeControlContext ctx)
        {
            RuntimeCtx = ctx;
        }

        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            _activeMoveSpeed = InputManager.Provider.Sprint ? runSpeed : walkSpeed;
            float x = InputManager.Provider.RawInputX;
            float z = InputManager.Provider.RawInputY;
            ctx.moveDirection =  new Vector3(x, 0.0f, z).normalized;

            NormalizedSpeed = _activeMoveSpeed * ctx.moveDirection.sqrMagnitude / runSpeed;

            ctx.moveDirection = MovementUtils.ToLocalDirectionWithProjectedForward(ctx.transform, ctx.viewCamera.transform.forward, ctx.moveDirection);
            ctx.lateralVelocity = ctx.moveDirection * _activeMoveSpeed * deltaTime;

            MovementUtils.CorrectCollisionProximityMotion(ctx.controller, ctx.settings.motionCorrectionLayers, ref ctx.lateralVelocity, 30);
            ctx.controller.Move(ctx.lateralVelocity);


#if BOB_LOGIC

            if (ctx.lateralVelocity.sqrMagnitude > 0)
            {
                float sample = Mathf.Sin(deltaTime * bobFreq);
                float sampledHeight = sample * bobHeight;
                ctx.controller.Move(new Vector3(0, sampledHeight, 0));
            }
#endif
        }
    }
}
