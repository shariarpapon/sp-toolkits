using System.Collections;
using UnityEngine;

namespace SPToolkits.TweeningUtils
{
    /// <summary>
    /// The base class for all types of tweeners.
    /// </summary>
    public abstract class Tweener : MonoBehaviour
    {
        /// <summary>
        /// Holds instantaneous immutable data about the a transform.
        /// </summary>
        protected struct TransformData
        {
            public readonly Vector3 position;
            public readonly Quaternion rotation;
            public readonly Vector3 localPosition;
            public readonly Vector3 localScale;
            public readonly Quaternion localRotation;

            public Vector3 EulerAngles { get { return rotation.eulerAngles; } }
            public Vector3 LocalEulerAngles { get { return localRotation.eulerAngles; } }

            public TransformData(Transform transform) :
            this(transform.position, transform.rotation, transform.localPosition, transform.localScale, transform.localRotation) { }

            public TransformData(Vector3 position, Quaternion rotation, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
            {
                this.position = position;
                this.rotation = rotation;

                this.localPosition = localPosition;
                this.localScale = localScale;
                this.localRotation = localRotation;
            }

            public static bool operator ==(TransformData a, TransformData b)
            {
                return a.position == b.position
                    && a.rotation == b.rotation
                    && a.localPosition == b.localPosition
                    && a.localScale == b.localScale
                    && a.localRotation == b.localRotation;
            }

            public static bool operator !=(TransformData a, TransformData b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == typeof(TransformData))
                    return (TransformData)obj == this;
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return string.Format("{0}{1}{2}{3}{4}", position, rotation, localPosition, localScale, localRotation).GetHashCode();
            }
        }

        [System.Serializable]
        public enum RunMethod { OnCall = 0, OnStart, OnEnable, OnUpdate }

        [SerializeField]
        protected RunMethod runMethod;
        [SerializeField]
        protected float timeMultiplier = 1;

        protected bool IsTweening { get; private set; }
        protected TransformData CapturedData { get; private set; }

        protected virtual void Awake() 
        {
            CaptureTransformData();
        }

        protected virtual void Start()
        {
            if (runMethod == RunMethod.OnStart) { Tween(); }
        }

        protected virtual void OnEnable()
        {
            if (runMethod == RunMethod.OnEnable) { Tween(); }
        }

        protected virtual void LateUpdate() 
        {
            if (runMethod == RunMethod.OnUpdate) { Tween(); }
        }

        private void OnDisable()
        {
            IsTweening = false;
            OnPostTween();
        }

        /// <summary>
        /// Called before the tweening routine starts.
        /// </summary>
        protected virtual void OnPreTween() { }

        /// <summary>
        /// Called after the tweening routine ends.
        /// </summary>
        protected virtual void OnPostTween() { }

        /// <summary>
        /// <para>The normalized time is passed in as argument.</para>
        /// <para>The time starts at 0 and increments every frame by (Time.deltaTime * timeMultiplier).</para>
        /// The tween routine ends when time is equal to 0.
        /// </summary>
        protected abstract IEnumerator OnTweenThisFrame(float time);


        private void PreTweenProcess()
        {
            OnPreTween();
        }

        private void PostTweenProcess() 
        {
            OnPostTween();
        }

        private void Tween() 
        {
            if (!IsTweening)
            {
                IsTweening = true;
                StartCoroutine(TweenRoutine());
            }
        }

        private IEnumerator TweenRoutine() 
        {
            PreTweenProcess();
            float time = 0;
            while (time < 1) 
            {
                yield return OnTweenThisFrame(time);
                time += Time.deltaTime * timeMultiplier;
                time = Mathf.Min(time, 1);
            }
            PostTweenProcess();
            IsTweening = false;
        }

        protected void CaptureTransformData() 
        {
            CapturedData = new TransformData(transform);
        }

        /// <summary>
        /// If the given component has a Tweener, it will invoke the tweening routine.
        /// </summary>
        public static void Run(Component obj) 
        {
            if (obj.TryGetComponent<Tweener>(out var tweener))
            {
                tweener.Tween();
            }
        }

        public void SetTimeMultiplier(float value) 
        {
            timeMultiplier = value;
        }

    }
}