using UnityEngine;

namespace SPToolkits.GameSettings
{
    [System.Serializable]
    public sealed class GameplaySettings
    {
        private const float _MAX_MOUSE_SENS_X = 500, _DEF_MOSUE_SENS_X = 400f;
        private const float _MAX_MOUSE_SENS_Y = 5, _DEF_MOUSE_SENS_Y = 4f;

        public float normMouseSensX;
        public float normMouseSensY;
        public bool camShakeEnabled;
        public bool notificationsEnabled;

        public GameplaySettings() 
        {
            normMouseSensX = _DEF_MOSUE_SENS_X / _MAX_MOUSE_SENS_X;
            normMouseSensY = _DEF_MOUSE_SENS_Y / _MAX_MOUSE_SENS_Y;
            camShakeEnabled = true;
            notificationsEnabled = true;
            Validate();
        }

        public void Validate() 
        {
            normMouseSensX = System.Math.Clamp(normMouseSensX, 0.0f, 1.0f);
            normMouseSensY = System.Math.Clamp(normMouseSensY, 0.0f, 1.0f);
        }

        public Vector2 GetEvaluatedMouseSensitivity()
          => new Vector2(normMouseSensX * _MAX_MOUSE_SENS_X, normMouseSensY * _MAX_MOUSE_SENS_Y);
    }
}
