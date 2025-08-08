using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SPToolkits.Extensions;
using SPToolkits.InputSystem;

namespace Wiz.Debugging
{
    public class DeveloperConsole : MonoBehaviour
    {
        private static DeveloperConsole _Instance;

        private static Dictionary<string, System.Action> _commands;

        [SerializeField]
        private GameObject console;
        [SerializeField]
        private Transform _logBox;
        [SerializeField]
        private GameObject _logEntry;
        [SerializeField]
        private TMP_InputField _input;

        public static void Init()
        {
            if (_Instance == null)
            {
                _Instance = _Instance.GetResourceInstance<DeveloperConsole>("Debug", "DeveloperConsole");
            }

            _commands = new Dictionary<string, System.Action>()
            {
                { "cls", _Instance.ClearLogs },
            };
        }

        private void Awake()
        {
            _input.onSubmit.AddListener(OnCommandSubmit);
            console.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (console.activeSelf)
                {
                    InputManager.Enable();
                    console.SetActive(false);
                }
                else
                {
                    InputManager.Disable();
                    console.SetActive(true);
                    FocusInputField();
                }
            }
        }
      
        public void OnCommandSubmit(string cmd) 
        {
            PushLog(cmd);

            if (_commands.ContainsKey(cmd.ToLower()))
                _commands[cmd]?.Invoke();

            _input.text = string.Empty;
            FocusInputField();
        }

        private void PushLog(string log) 
        {
            log = "> " + log;
            Instantiate(_logEntry, _logBox).GetComponent<TextMeshProUGUI>().text = log;
        }

        private void FocusInputField()
        {
            _input.ActivateInputField();
        }

        private void ClearLogs() 
        {
            foreach (Transform t in _logBox)
                Destroy(t.gameObject);
        }
    }
}