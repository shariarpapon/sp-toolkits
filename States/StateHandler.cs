using UnityEngine;

namespace SPToolkits.States
{
    public abstract class StateHandler<T> : MonoBehaviour where T : StateHandler<T>
    {
        protected IState<T> _currentState;

        public virtual void ChangeState(IState<T> newState) 
        {
            var stateHandler = (T)this;
            if (_currentState != null)
                _currentState.ExitState(stateHandler);

            _currentState = newState;
            _currentState.EnterState(stateHandler);
        }
    }
}                                