using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SPToolkits.Extensions.Unity;

namespace SPToolkits.UIUtils
{
    public class LoadingScreen : MonoBehaviour
    {
        private static LoadingScreen _Instance;
        public Image progressBar;
        public TextMeshProUGUI progress;

        public static void Init() 
        {
            if (_Instance == null)
                _Instance = _Instance.GetResourceInstance<LoadingScreen>("UI Prefabs", "LoadingScreen");

            _Instance.progressBar.type = Image.Type.Filled;
            _Instance.progressBar.fillAmount = 0;
            _Instance.progress.text = "0%";
            _Instance.gameObject.SetActive(true);
        }

        public static void SetProgress(float value) 
        {
            _Instance.progress.text = Mathf.RoundToInt(value * 100.0f).ToString() + "%";
            _Instance.progressBar.fillAmount = value;
        }

        public static void Close(bool destroy = true)
        {
            if (destroy)
                Destroy(_Instance.gameObject);
            else
                _Instance.gameObject.SetActive(false);
        }

        public static void DestroyIfExists() 
        {
            if (_Instance != null)
                Destroy(_Instance.gameObject);
        }
    }
}