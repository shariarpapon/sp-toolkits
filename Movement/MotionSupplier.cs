using UnityEngine;

namespace SPToolkits.Movement
{
    public abstract class MotionSupplier : ScriptableObject
    {
        public bool IsEnabled => _isEnabled;

        [SerializeField] private bool _isEnabled = true;

        public void Tick(float deltaTime, RuntimeControlContext ctx)
        {
            if (!_isEnabled)
                return;

            _Tick(deltaTime, ctx);
        }

        public virtual void Init(RuntimeControlContext ctx) { }

        protected abstract void _Tick(float deltaTime, RuntimeControlContext ctx);

        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
        }
    }
}