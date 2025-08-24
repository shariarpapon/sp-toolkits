using UnityEngine;
using SPToolkits.InputSystem;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName =nameof(PlayerJump), menuName = "SPToolkits/Motion Suppliers/" + nameof(PlayerJump))]
    public class PlayerJump : MotionSupplier
    {
        [System.Serializable]
        public class JumpConfig
        {
            public float jumpHeight = 1;
            public float gravityMultiplier = 1;
            public float JumpSpeed { get; private set; }

            public void CalculateJumpSpeed(float gravity) 
            {
                JumpSpeed = MovementUtils.CalculateJumpSpeed(gravity * gravityMultiplier, jumpHeight);
            }
        }

        public JumpConfig[] jumps;
        //public float firstJumpHeight = 1.5f;
        //public float secondJumpHeight = 2.0f;
        public float maxInputBufferTime = 0.15f;
        public float coyoteTime = 0.15f;

        //private float _firstJumpSpeed = 3;
        private float _coyoteTimer = 0;
        private float _inputBufferTime = 0;

        private int _maxJumpCount = 0;
        private int _jumpCounter = 0;
        private bool _jumpTriggered = false;

        private RuntimeControlContext _ctx;

        /// <summary>
        /// Invoked upon a jump being triggered.
        /// <br>Parameter is the current jump counter value.</br>
        /// </summary>
        public event System.Action<int> onJump;

        public override void Init(RuntimeControlContext ctx)
        {
            _ctx = ctx;
            InitJumps();
        }

        public void InitJumps() 
        {
            _maxJumpCount = jumps.Length;
            for (int i = 0; i < jumps.Length; i++)
                jumps[i].CalculateJumpSpeed(_ctx.settings.gravity);
        }

        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            if (ctx.isCenterGrounded && Vector3.Dot(ctx.verticalVelocity.normalized, ctx.LocalUp) <= 0f)
            {
                _coyoteTimer = coyoteTime;
                _inputBufferTime = 0;
                _jumpCounter = 0;
            }
            else
                _coyoteTimer -= deltaTime;

            JumpLogic(ctx, deltaTime);
        }

        private void JumpLogic(RuntimeControlContext ctx, float deltaTime)
        {
            //Why is it necessary Jump input twice? idk you tell me why it just works. 
            if (InputManager.Provider.Jump)
                _inputBufferTime = Time.time;

            _jumpTriggered = Time.time - _inputBufferTime < maxInputBufferTime;
            _jumpCounter = Mathf.Clamp(_jumpCounter, 0, jumps.Length);
            if (InputManager.Provider.Jump && _jumpTriggered)
                if (ctx.isCenterGrounded || _coyoteTimer > 0 || _jumpCounter < jumps.Length)
                {
                    ctx.verticalVelocity = jumps[_jumpCounter].JumpSpeed * ctx.transform.up;
                    _jumpCounter++;
                    onJump?.Invoke(_jumpCounter);
                }
        }

       
    }   
}