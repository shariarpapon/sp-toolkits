using UnityEngine;
using SPToolkits.InputSystem;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName ="Player Jump", menuName = "SPToolkits/Motion Suppliers/Player Jump")]
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

        public override void Init(MotionControlContext ctx)
        {
            //TODO: Gravity hardcoded, might be smart to move gravity to global data struct.
            _jumpSpeed = MovementUtils.CalculateJumpSpeed(22f, peakJumpHeight);
        }

        protected override void Tick(float deltaTime, MotionControlContext ctx)
        {
            if (ctx.isCenterGrounded && Vector3.Dot(ctx.verticalVelocity.normalized, ctx.transform.up) <= 0f)
            {
                _coyoteTimer = coyoteTime;
                _inputBufferTime = 0;
                _jumpCount = 0;
            }
            else
                _coyoteTimer -= deltaTime;

            JumpLogic(ctx, deltaTime);
        }

        private void JumpLogic(MotionControlContext ctx, float deltaTime)
        {
            if (InputManager.InputProvider.Jump)
                _inputBufferTime = Time.time;

            bool jumpTriggered = Time.time - _inputBufferTime < maxInputBufferTime;

            if (InputManager.InputProvider.Jump && jumpTriggered)
                if (ctx.isCenterGrounded || _coyoteTimer > 0 || _jumpCount < maxJumpCount)
                {
                    _jumpCount++;
                    ctx.verticalVelocity = _jumpSpeed * ctx.transform.up;
                    Debug.Log("jumpcount: " + _jumpCount);
                }
        }
    }
}