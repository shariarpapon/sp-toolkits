using System.Collections;
using UnityEngine;

namespace SPToolkits.TweeningUtils
{
    public sealed class RotationTweener : Tweener
    {        
        [SerializeField]
        private Vector3 _startAngleOffset;
        [SerializeField]
        private Vector3 _targetAngleOffset;
        private Vector3 _targetEulerAngles;

        protected sealed override void Awake()
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles + _startAngleOffset);
            CaptureTransformData();
        }

        protected sealed override void OnPreTween()
        {
            _targetEulerAngles = transform.eulerAngles + _targetAngleOffset;
        }

        protected sealed override void OnPostTween()
        {
            transform.rotation = Quaternion.Euler(CapturedData.EulerAngles + new Vector3(0, 360, 0));
        }

        protected sealed override IEnumerator OnTweenThisFrame(float time)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(CapturedData.EulerAngles, _targetEulerAngles, time));
            yield return null;
        }
    }
}