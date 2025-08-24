using UnityEngine;

namespace SPToolkits.InputSystem
{
    public sealed class DisabledInputProvider : InputProvider
    {
        //Menu
        public override bool PauseGame => Input.GetKeyDown(KeyCode.Escape);
        public override bool TabMenu => Input.GetKeyDown(KeyCode.Tab);
        public override bool OpenCommandConsole => Input.GetKeyDown(KeyCode.F1);
        public override bool Submit => Input.GetKey(KeyCode.Return);
   
        //Interaction
        public override bool PrimaryUse => false;
        public override bool SpecialUse => false;
        public override bool Interact => false;
        public override bool SelectNextQuickSlotItem => false;


        //Movement
        public override float RawInputX => 0;
        public override float RawInputY => 0;
        public override float InputX => 0;
        public override float InputY => 0;
        public override Vector2 MousePosition => Vector2.zero;
        public override Vector2 MouseScrollDelta => Vector2.zero;
        public override float MouseX => 0;
        public override float MouseY => 0;

        //Special Movement
        public override bool Jump => false;
        public override bool Sprint => false;
        public override bool Aim => false;
        public override bool Dash => false;
        public override bool Forcedown => false;
    }
}