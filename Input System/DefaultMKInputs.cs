using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace SPToolkits.InputSystem
{
    /// <summary>
    /// The default keyboard/mouse input provider.
    /// </summary>
    public sealed class DefaultMKInputs : IInputProvider
    {
        public bool PauseGame => UnityInput.GetKeyDown(KeyCode.Escape);
        public bool TabMenu => UnityInput.GetKeyDown(KeyCode.Tab);

        public bool OpenCommandConsole => UnityInput.GetKeyDown(KeyCode.F1);
        public bool Submit => UnityInput.GetKeyDown(KeyCode.Return);

        public bool Jump => UnityInput.GetKeyDown(KeyCode.Space);
        public bool Sprint => UnityInput.GetKey(KeyCode.LeftShift);
        public bool Aim => UnityInput.GetMouseButton(1);
        public bool Dash => UnityInput.GetKeyDown(KeyCode.F);
        public bool PrimaryUse => UnityInput.GetMouseButtonDown(0);
        public bool SpecialUse => UnityInput.GetKeyDown(KeyCode.F);
        public bool Interact => UnityInput.GetKeyDown(KeyCode.E);
        public bool SelectNextQuickSlotItem => UnityInput.GetKeyDown(KeyCode.X);

        public float RawInputX => UnityInput.GetAxisRaw("Horizontal");
        public float RawInputY => UnityInput.GetAxisRaw("Vertical");
        public float InputX => UnityInput.GetAxis("Horizontal");
        public float InputY => UnityInput.GetAxis("Vertical");
        public Vector2 MousePosition => UnityInput.mousePosition;
        public Vector2 MouseScrollDelta => UnityInput.mouseScrollDelta;
        public float MouseX => UnityInput.GetAxis("Mouse X");
        public float MouseY => UnityInput.GetAxis("Mouse Y");
    }
}