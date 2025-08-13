using System.Collections;
using UnityEngine;

namespace SPToolkits.TweeningUtils
{
    public sealed class ScaleTweener : Tweener
    {
        [SerializeField]
        private AnimationCurve _curve;

        protected sealed override void OnPreTween()
        {
            CaptureTransformData();
        }

        protected sealed override void OnPostTween()
        {
            transform.localScale = _curve.Evaluate(1) * CapturedData.localScale;
        }

        protected sealed override IEnumerator OnTweenThisFrame(float time) 
        { 
            transform.localScale = Vector3.Lerp(CapturedData.localScale, CapturedData.localScale * _curve.Evaluate(time), time);
            yield return null;

        }
    }
}
