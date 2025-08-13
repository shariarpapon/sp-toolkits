using UnityEngine;

namespace SPToolkits.InputSystem
{ 
    /// <summary>
    /// This interface contains all input keys needed for the game.
    /// </summary>
    public interface IInputProvider
    {
        //
        public bool PauseGame { get; }
        public bool TabMenu { get; }
        public bool OpenCommandConsole { get; }
        public bool Submit { get; }

        //Gameplay
        public bool Jump { get; } 
        public bool Sprint { get; }
        public bool PrimaryUse { get; }
        public bool SpecialUse { get; }
        public bool Aim { get; }
        public bool Dash { get; }
        public bool Interact { get; }
        public bool SelectNextQuickSlotItem { get; }

        //General
        public float RawInputX { get; }
        public float RawInputY { get; }
        public float InputX { get; }
        public float InputY { get; }
        public Vector2 MousePosition { get; } 
        public Vector2 MouseScrollDelta { get; }
        public float MouseX { get; }
        public float MouseY { get; }
    }
}
