using UnityEngine;

namespace SPToolkits.States
{
    /// <summary>
    /// A singleton state handler deriving from the normal StateHandler.
    /// </summary>
    public abstract class SingletonStateHandlerMonoBehaviour<T> : StateHandlerMonoBehaviour<T> where T : SingletonStateHandlerMonoBehaviour<T>
    {
        public static T Instance { get; private set; }

        /// <summary>
        /// <br>Called once before the initialization of the singleton.
        /// <br/>This will be called anytime the singleton's Awake method is called.
        /// <br/>This maybe called more than once in the lifetime of the singleton.</br>
        /// </summary>
        protected virtual void OnPreAwake() { }

        protected virtual void Awake()
        {
            OnPreAwake();
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            OnPostAwake();
        }

        /// <summary>
        /// Called once the singleton instance is successfully awakened.
        /// <br>It's only called once in the lifetime of the singleton.</br>
        /// </summary>
        protected virtual void OnPostAwake() { }
    }
}