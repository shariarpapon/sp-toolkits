using System;

namespace SPToolkits.GameSettings
{
    [System.Serializable]
    public sealed class QualitySettings
    {//
        private struct NumericBound
        {
            public float min;
            public float max;
            public NumericBound(float min, float max) 
            {
                this.min = min;
                this.max = max;
            }

            /// <param name="value">Value to evaluate.</param>
            /// <returns>The evaluated value that is bounded between the min and max.</returns>
            public float Evaluate(float value)
                => Math.Clamp(value, min, max);

            /// <param name="value">Value to evaluate.</param>
            /// <returns>The evaluated value that is bounded between the min and max.</returns>
            public int Evaluate(int value)
                => (int)Math.Clamp(value, min, max);
        }

        [System.Serializable]
        public enum QualityLevel 
        {
            Mexican = 0,
            Low = 1,
            Medium = 2,
            High = 3,
        }

        public QualityLevel quality;
        public bool postProcessEnabled;
        public bool bloomEnabled;
        public bool vignetteEnabled;
        public bool tonemappingEnabled;
        public int shadowCascades;
        public float shadowDistance;

        private static readonly NumericBound
            _ShadowCascadesBound = new NumericBound(1, 4),
            _ShadowDistanceBound = new NumericBound(2, 256);


        public QualitySettings(QualityLevel quality = QualityLevel.High) 
        {
            this.quality = quality;
            SetAndOverrideQualityLevel(quality);
        }

        /// <summary>
        /// Sets the quality level but does not override the settings.
        /// </summary>
        /// <param name="level">New quality level.</param>
        public void SetQualitLevel(QualityLevel level)
        {
            quality = level;
        }

        /// <summary>
        /// Sets the quality level and overrides the settings.
        /// </summary>
        /// <param name="level">New quality level.</param>
        public void SetAndOverrideQualityLevel(QualityLevel level) 
        {
            SetQualitLevel(level);
            switch (level)
            {
                case QualityLevel.Mexican:
                    postProcessEnabled = true;
                    bloomEnabled = false;
                    vignetteEnabled = false;
                    tonemappingEnabled = true;
                    shadowCascades = 1;
                    shadowDistance = 20f;
                    break;
                case QualityLevel.Low:
                    postProcessEnabled = true;
                    bloomEnabled = false;
                    vignetteEnabled = false;
                    tonemappingEnabled = false;
                    shadowCascades = 1;
                    shadowDistance = 50f;
                    break;
                case QualityLevel.Medium:
                    postProcessEnabled = true;
                    bloomEnabled = true;
                    vignetteEnabled = false;
                    tonemappingEnabled = true;
                    shadowCascades = 2;
                    shadowDistance = 100f;
                    break;
                case QualityLevel.High:
                    postProcessEnabled = true;
                    bloomEnabled = true;
                    vignetteEnabled = true;
                    tonemappingEnabled = true;
                    shadowCascades = 4;
                    shadowDistance = 200f;
                    break;
            }
            Validate();
        }

        public void Validate() 
        {
            shadowCascades = _ShadowCascadesBound.Evaluate(shadowCascades);
            shadowDistance = _ShadowDistanceBound.Evaluate(shadowDistance);
        }
    }
}