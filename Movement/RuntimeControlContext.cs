using UnityEngine;

namespace SPToolkits.Movement
{
    public class RuntimeControlContext
    {
        //Dependencies
        public readonly CharacterControllerMotionHandler handler = null; 
        public readonly CharacterController controller = null;
        public readonly Transform transform = null;
        public readonly Camera viewCamera = null;
        public readonly GlobalMovementSettings settings = null;

        //Helper properties
        public Vector3 LocalUp => transform.up;
        public Vector3 LocalForward => transform.forward;

        //Runtime
        public bool isCenterGrounded = false;
        public Vector3 moveDirection = Vector3.zero;
        public Vector3 lateralVelocity = Vector3.zero;
        public Vector3 verticalVelocity = Vector3.zero;

        public RuntimeControlContext(CharacterControllerMotionHandler handler, CharacterController controller, GlobalMovementSettings settings, Camera viewCamera) 
        {
            this.handler = handler;
            this.controller = controller;
            this.settings = settings;
            this.viewCamera = viewCamera;
            transform = controller.transform;
        }
    }
}