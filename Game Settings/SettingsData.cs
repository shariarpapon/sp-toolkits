namespace SPToolkits.GameSettings
{
    [System.Serializable]
    public sealed class SettingsData
    {
        public QualitySettings qualitySettings = null;
        public GameplaySettings gameplaySettings = null;

        /// <summary>
        /// Default initializes all the settings and applies them.
        /// </summary>
        public SettingsData() 
        {
            gameplaySettings = new GameplaySettings();
            qualitySettings = new QualitySettings();
        }

        /// <summary>
        /// Validate all the settings
        /// </summary>
        public void ValidateSettings() 
        {
            gameplaySettings.Validate();
            qualitySettings.Validate();
        }
    }
}