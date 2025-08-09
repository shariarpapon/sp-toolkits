using SPToolkits.InputSystem;
using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = "Player Dash", menuName = "Wiz/Motion Suppliers/Player Dash")]
    public class PlayerDash : MotionSupplier
    {
        private enum DashState 
        { 
            Ready, 
            Dashing, 
            Cooldown,
            Falling
        }

        public float dashDistance = 4f;
        public float dashSpeed = 30f;
        public float dashCooldown = 0.2f;

        private DashState _dashState = DashState.Ready;
        private float _remainingDashDistance = 0.0f;
        private float _dashCooldownTimer = 0.0f;

        protected override void _Tick(float deltaTime, MotionControlContext ctx)
        {
            switch (_dashState)
            {
                case DashState.Ready:
                    if (InputManager.InputProvider.Dash)
                        StartDash(ctx);
                    break;
                case DashState.Dashing:
                    Dash(deltaTime, ctx);
                    break;
                case DashState.Cooldown:
                    Cooldown(deltaTime, ctx);
                    break;
                case DashState.Falling:
                    Falling(ctx);
                    break;
            }
        }

        private void StartDash(MotionControlContext ctx) 
        {
            _dashState = DashState.Dashing;
            _remainingDashDistance = dashDistance;

            ctx.handler.SetMotionSupplierEnabled(typeof(PLayerLateralMovement), false);
        }

        private void Dash(float deltaTime, MotionControlContext ctx)
        {
            float step = dashSpeed * deltaTime;
            if (step >= _remainingDashDistance)
                step = _remainingDashDistance;

            ctx.controller.Move(ctx.LocalForward * step);
            _remainingDashDistance -= step;

            if (_remainingDashDistance <= 0)
                EndDash(ctx);
        }

        private void EndDash(MotionControlContext ctx)
        {
            _dashState = DashState.Cooldown;
            _dashCooldownTimer = dashCooldown;
            ctx.handler.SetMotionSupplierEnabled(typeof(PLayerLateralMovement), true);
        }

        private void Cooldown(float deltaTime, MotionControlContext ctx) 
        {
            _dashCooldownTimer -= deltaTime;
            if (_dashCooldownTimer <= 0) 
            {
                _dashCooldownTimer = 0;

                if (ctx.isCenterGrounded)
                    _dashState = DashState.Ready;
                else
                    _dashState = DashState.Falling;
            }
        }

        private void Falling(MotionControlContext ctx) 
        {
            if (ctx.isCenterGrounded)
                _dashState = DashState.Ready;
        }

   
    }
}