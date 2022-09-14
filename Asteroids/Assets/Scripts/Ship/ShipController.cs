using UnityEngine;

namespace Ship
{
    public class ShipController : IController
    {
        private IMoveRotateInputData _inputSystem;

        private ShipModel _model;
        private ShipView _view;
        private ShipTransformHandler _shipTransformHandler;
        private CollisionHandler _collisionHandler;

        public System.Action OnShipDestroy { get; set; }

        public ShipController(ShipConfig shipConfig, ShipView view, IMoveRotateInputData inputs, CollisionHandler collisionHandler)
        {
            _shipTransformHandler = new ShipTransformHandler(shipConfig);
            _inputSystem = inputs;

            _model = new ShipModel(shipConfig.StartPosition, shipConfig.CollisionRadius);
            _view = view;
            _collisionHandler = collisionHandler;
            _collisionHandler.AddCollision(_model);
            OnShipDestroy = _model.OnCollision;
        }

        void IController.Update()
        {
            UpdateAcceleration();
            UpdatePosition();

            if (_inputSystem.RotationValue == 0)
                return;
            UpdateRotation();
        }

        public ModelTransform GetShipTransform() => _model.modelTransform;

        private void UpdateAcceleration()
        {
            if (_inputSystem.MoveForwardPhase == UnityEngine.InputSystem.InputActionPhase.Performed)
                _shipTransformHandler.IncreaseAcceleration(_model.modelTransform.Rotation, Time.deltaTime);
            else
                _shipTransformHandler.DecreaseAcceleration(Time.deltaTime);
        }
        private void UpdatePosition()
        {
            _shipTransformHandler.ChangePosition(_model.modelTransform);
            _view.ChangePosition(_model.modelTransform.Position);
        }

        private void UpdateRotation()
        {
            _shipTransformHandler.ChangeRotation(_model.modelTransform, _inputSystem.RotationValue, Time.deltaTime);
            _view.ChangeRotation(_model.modelTransform.Rotation);
        }
    }
}