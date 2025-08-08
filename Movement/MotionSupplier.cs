using UnityEngine;

namespace SPToolkits.Movement
{
    public abstract class MotionSupplier : ScriptableObject
    {
        public bool isEnabled = true;

        public void Step(float deltaTime, MotionControlContext ctx)
        {
            if (!isEnabled)
                return;
            Tick(deltaTime, ctx);
        }

        public virtual void Init(MotionControlContext ctx) { }

        protected abstract void Tick(float deltaTime, MotionControlContext ctx);
    }
}