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
        private MotionSupplier _lateralMovement;

        public override void Init(RuntimeControlContext ctx)
        {
            _lateralMovement = ctx.handler.GetMotionSupplierOfType(typeof(PlayerLateralMovement));
        }

        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            switch (_dashState)
            {
                case DashState.Ready:
                    if (InputManager.Provider.Dash)
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

        private void StartDash(RuntimeControlContext ctx) 
        {
            _dashState = DashState.Dashing;
            _remainingDashDistance = dashDistance;
            _lateralMovement.SetEnabled(false);
        }

        private void Dash(float deltaTime, RuntimeControlContext ctx)
        {
            float step = dashSpeed * deltaTime;
            if (step >= _remainingDashDistance)
                step = _remainingDashDistance;

            ctx.controller.Move(ctx.LocalForward * step);
            _remainingDashDistance -= step;

            if (_remainingDashDistance <= 0)
                EndDash(ctx);
        }

        private void EndDash(RuntimeControlContext ctx)
        {
            _dashState = DashState.Cooldown;
            _dashCooldownTimer = dashCooldown;
            _lateralMovement.SetEnabled(true);
        }

        private void Cooldown(float deltaTime, RuntimeControlContext ctx) 
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

        private void Falling(RuntimeControlContext ctx) 
        {
            if (ctx.isCenterGrounded)
                _dashState = DashState.Ready;
        }

   
    }
}