using System;
using System.Collections.Generic;
using UnityEngine;

namespace SPToolkits.Movement
{
    public sealed class CharacterControllerMotionHandler
    {
        public bool motionSupplyEnabled = true;

        public RuntimeControlContext Context { get { return _context; } }
        private readonly RuntimeControlContext _context;
        private readonly MotionSupplier[] _motionSuppliers;

        private readonly Dictionary<Type, MotionSupplier> _motionSupplierCache;


        public CharacterControllerMotionHandler(CharacterController controller, GlobalMovementSettings settings, MotionSupplier[] motionSuppliers, Camera viewCamera) 
        {
            _motionSuppliers = motionSuppliers;
            _context = new RuntimeControlContext(this, controller, settings, viewCamera);

            _motionSupplierCache = new Dictionary<Type, MotionSupplier>();
            for (int i = 0; i < _motionSuppliers.Length; i++)
            {
                Type motionSupplierType = _motionSuppliers[i].GetType();
                if (!_motionSupplierCache.ContainsKey(motionSupplierType))
                    _motionSupplierCache.Add(motionSupplierType, _motionSuppliers[i]);
                else Debug.LogError("Duplicate motion suppliers provided, it will be skipped.");
            }
        }

        public CharacterControllerMotionHandler(CharacterController controller, GlobalMovementSettings settings, MotionSupplier[] motionSuppliers)
        {
            _motionSuppliers = motionSuppliers;
            _context = new RuntimeControlContext(this, controller, settings, Camera.main);
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

        public void SetMotionSupplierEnabled(Type typeOfMotionSupplier, bool enabled)
        {
            if (_motionSupplierCache.ContainsKey(typeOfMotionSupplier))
                _motionSupplierCache[typeOfMotionSupplier].SetEnabled(enabled);
        }

        public MotionSupplier GetMotionSupplierOfType(Type typeOfSupplier)
        {
            if (_motionSupplierCache.ContainsKey(typeOfSupplier))
                return _motionSupplierCache[typeOfSupplier];
            else
            {
                Debug.LogError("Motion Supplier type does not exist in the cachce.");
                return null;
            }
        }


        public T GetMotionSupplierOfType<T>() where T : MotionSupplier
        {
            if (_motionSupplierCache.ContainsKey(typeof(T)))
                return _motionSupplierCache[typeof(T)] as T;
            else 
            { 
                Debug.LogError("Motion Supplier type does not exist in the cachce.");
                return null;
            }
        }

        private void UpdateContext(RuntimeControlContext ctx) 
        {
            ctx.isCenterGrounded = MovementUtils.IsCenterGrounded(ctx.controller, ctx.settings.whatsIsGround, ctx.settings.maxStableSlopeAngle);
        }

        private void SupplyMotion(RuntimeControlContext ctx) 
        {
            for (int i = 0; i < _motionSuppliers.Length; i++)
                _motionSuppliers[i].Tick(Time.deltaTime, ctx);
        }

    }
        
}