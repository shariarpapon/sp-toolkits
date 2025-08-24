using UnityEngine;

namespace SPToolkits.States
{
    public abstract class StateHandlerMonoBehaviour<T> : MonoBehaviour where T : StateHandlerMonoBehaviour<T>
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