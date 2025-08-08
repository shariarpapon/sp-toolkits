using System;

namespace SPToolkits.States
{
    public interface IState<T> where T : StateHandler<T>
    {
        public void EnterState(T handler);
        public void UpdateState(T handler);
        public void ExitState(T handler);
    }
}