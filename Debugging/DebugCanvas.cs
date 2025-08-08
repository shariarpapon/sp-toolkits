using TMPro;
using UnityEngine;
using SPToolkits.Extensions;

namespace Wiz.Debugging
{
    public sealed class DebugCanvas : MonoBehaviour
    {
        private static DebugCanvas _Instance;
        public static DebugCanvas Instance 
        {
            get 
            {
                if (_Instance == null)
                    _Instance = _Instance.GetResourceInstance<DebugCanvas>("Debug", "DebugCanvas");
                return _Instance;
            }
        }

        [SerializeField]
        private Transform textBox;
        [SerializeField]
        private GameObject textRef;
        
        public static void PushText(string text, Color color)
        {
            var tmp = Instantiate(Instance.textRef, Instance.textBox).GetComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.color = color;
            Canvas.ForceUpdateCanvases();
        }

        public static void Clear() 
        {
            foreach (Transform t in Instance.textBox)
                Destroy(t.gameObject);
        }

        public static void PushEmptyLine() => PushText("\n");

        public static void PushText(string text) => PushText(text, Color.white * 0.9f);

        public static void Show() => Instance.gameObject.SetActive(true);

        public static void Hide() => Instance.gameObject.SetActive(false);
        
    }
}