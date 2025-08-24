using SPToolkits.InputSystem;
using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = nameof(PlayerDescent), menuName = "SPToolkits/Motion Suppliers/"+ nameof(PlayerDescent))]
    public class PlayerDescent: MotionSupplier
    {
        private enum DescentState { Ready = 0, Descending, Cooldown }
        public float terminalSpeed = 15f;
        public float cooldown = 0.65f;

        [Space]
        public bool interpolateSpeed = true;
        public float duration = 0.5f;
        public AnimationCurve interpolationCurve;

        public event System.Action<float> onLand;

        private RuntimeControlContext _ctx;
        private PlayerLateralMovement _lateralMovement;
        private PlayerVerticalMovement _verticalMovement;
        private PlayerJump _jump;
        private PlayerDash _dash;

        private DescentState _state = DescentState.Ready;
        private float _cooldownTimer = 0;
        private float _terminalSpeedDt = 0;
        private float _speed;
        private Vector3 _startPosition = Vector3.zero;

        public override void Init(RuntimeControlContext ctx)
        {
            _ctx = ctx;
            _lateralMovement = ctx.handler.GetMotionSupplierOfType<PlayerLateralMovement>();
            _verticalMovement = ctx.handler.GetMotionSupplierOfType<PlayerVerticalMovement>();
            _jump = ctx.handler.GetMotionSupplierOfType<PlayerJump>();
            _dash = ctx.handler.GetMotionSupplierOfType<PlayerDash>();
        }

        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
            switch (_state) 
            {
                case DescentState.Ready:
                    if (InputManager.Provider.Forcedown && !ctx.isCenterGrounded)
                        StartDescent();
                    break;
                case DescentState.Descending:
                    Descend(deltaTime);
                    break;
                case DescentState.Cooldown:
                    Cooldown(deltaTime);
                    break;
            }
        }

        private void StartDescent() 
        {
            _verticalMovement.SetGravityScale(0);
            _lateralMovement.SetEnabled(false);
            _dash.SetEnabled(false);
            _jump.SetEnabled(false);
            _terminalSpeedDt = 0;
            _ctx.verticalVelocity = Vector3.zero;
            _startPosition = _ctx.transform.position;
            _state = DescentState.Descending;
        }

        private void Descend(float deltaTime) 
        {
            if (_ctx.isCenterGrounded)
            {
                EndDescent();
                return;
            }

            _terminalSpeedDt += deltaTime;
            if (_terminalSpeedDt > duration)
                _terminalSpeedDt = duration;

            _speed = interpolateSpeed ? terminalSpeed * interpolationCurve.Evaluate(_terminalSpeedDt / duration) : terminalSpeed;
            _ctx.verticalVelocity -= _speed * _ctx.LocalUp;
        }

        private void Cooldown(float deltaTime) 
        {
            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
                _state = DescentState.Ready;
                return;
            }
            _cooldownTimer -= deltaTime;
        }

        private void EndDescent()
        {
            _verticalMovement.SetGravityScale(1);
            _lateralMovement.SetEnabled(true);
            _dash.SetEnabled(true);
            _jump.SetEnabled(true);
            onLand?.Invoke((_ctx.transform.position - _startPosition).magnitude);
            
            _cooldownTimer = cooldown;
            _state = DescentState.Cooldown;
        }
    }
}
            