using UnityEngine;
using SPToolkits.InputSystem;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName ="Player Jump", menuName ="Wiz/Motion Suppliers/Player Jump")]
    public class PlayerJump : MotionSupplier
    {
        public float peakJumpHeight = 1.5f;
        public float maxInputBufferTime = 0.15f;
        public float coyoteTime = 0.15f;
        public int maxJumpCount = 2;

        private float _jumpSpeed = 3;
        private float _coyoteTimer = 0;
        private float _inputBufferTime = 0;

        private int _jumpCount = 0;
        private bool _jumpTriggered = false;

        public override void Init(RuntimeControlContext ctx)
        {
            CalculateJumpSpeed(ctx.settings.gravity);
        }

        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            if (ctx.isCenterGrounded && Vector3.Dot(ctx.verticalVelocity.normalized, ctx.LocalUp) <= 0f)
            {
                _coyoteTimer = coyoteTime;
                _inputBufferTime = 0;
                _jumpCount = 0;
            }
            else
                _coyoteTimer -= deltaTime;

            JumpLogic(ctx, deltaTime);
        }

        private void JumpLogic(RuntimeControlContext ctx, float deltaTime)
        {
            if (InputManager.Provider.Jump)
                _inputBufferTime = Time.time;

            _jumpTriggered = Time.time - _inputBufferTime < maxInputBufferTime;

            if (_jumpTriggered)
                if (ctx.isCenterGrounded || _coyoteTimer > 0 || _jumpCount < maxJumpCount)
                {
                    _jumpCount++;
                    ctx.verticalVelocity = _jumpSpeed * ctx.LocalUp;
                }
        }

        public void CalculateJumpSpeed(float gravity) 
        {
            _jumpSpeed = MovementUtils.CalculateJumpSpeed(gravity, peakJumpHeight);
        }
    }   
}