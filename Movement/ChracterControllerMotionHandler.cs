using UnityEngine;

namespace SPToolkits.Movement
{
    public sealed class ChracterControllerMotionHandler
    {
        public bool motionSupplyEnabled = true;

        private readonly MotionControlContext _context;
        private readonly MotionSupplier[] _motionSuppliers;

        public ChracterControllerMotionHandler(CharacterController controller, GlobalMovementSettings settings, MotionSupplier[] motionSuppliers, Camera viewCamera) 
        {
            _motionSuppliers = motionSuppliers;
            _context = new MotionControlContext(controller, settings, viewCamera);
        }

        public ChracterControllerMotionHandler(CharacterController controller, GlobalMovementSettings settings, MotionSupplier[] motionSuppliers)
        {
            _motionSuppliers = motionSuppliers;
            _context = new MotionControlContext(controller, settings, Camera.main);
        }

        public void Init()
        {
            for (int i = 0; i < _motionSuppliers.Length; i++)
                _motionSuppliers[i].Init(_context);
        }
        public void Dispose() { }

        public void Update()
        {
            if (!motionSupplyEnabled)
                return;

            UpdateContext(_context);
            SupplyMotion(_context);
        }

        private void UpdateContext(MotionControlContext ctx) 
        {
            ctx.isCenterGrounded = MovementUtils.IsCenterGrounded(ctx.controller, ctx.settings.groundLayers, ctx.settings.maxStableSlopeAngle);
        }

        private void SupplyMotion(MotionControlContext ctx) 
        {
            for (int i = 0; i < _motionSuppliers.Length; i++)
                _motionSuppliers[i].Step(Time.deltaTime, ctx);
        }

    }
        
}