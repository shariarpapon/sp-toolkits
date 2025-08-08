using UnityEngine;

namespace SPToolkits.InputSystem
{
    public sealed class DisabledInputs : IInputProvider
    {
        public bool PauseGame => Input.GetKeyDown(KeyCode.Escape);
        public bool TabMenu => Input.GetKeyDown(KeyCode.Tab);
        public bool Jump => false;
        public bool Sprint => false;
        public bool Aim => false;
        public bool Dash => false;
        public bool PrimaryUse => false;
        public bool SpecialUse => false;
        public bool Interact => false;
        public bool SelectNextQuickSlotItem => false;

        public float RawInputX => 0;
        public float RawInputY => 0;
        public float InputX => 0;
        public float InputY => 0;
        public Vector2 MousePosition => Vector2.zero;
        public Vector2 MouseScrollDelta => Vector2.zero;
        public float MouseX => 0;
        public float MouseY => 0;
    }
}