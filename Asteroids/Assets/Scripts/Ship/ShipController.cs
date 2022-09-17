using UnityEngine;
using UnityEngine.Events;

namespace Ship
{
    public class ShipController : IUpdatable
    {
        private IMoveRotateInputData _inputSystem;

        private ShipModel _model;
        private ShipView _view;
        private ShipTransformHandler _shipTransformHandler;
        private CollisionHandler _collisionHandler;

        public UnityEvent OnShipDestroy { get; private set; }

        public ShipController(ShipConfig shipConfig, ShipView view, IMoveRotateInputData inputs, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _shipTransformHandler = new ShipTransformHandler(shipConfig, cameraData);
            _inputSystem = inputs;

            _model = new ShipModel(shipConfig.StartPosition, shipConfig.CollisionRadius);
            _view = view;
            _collisionHandler = collisionHandler;
            _collisionHandler.AddCollision(_model);

            OnShipDestroy = new UnityEvent();
            _model.OnCollision.AddListener(onShipCollision);
        }

        public float GetCurrentSpeed() => _shipTransformHandler.CurrentSpeed;

        void IUpdatable.Update()
        {
            UpdateAcceleration();
            UpdatePosition();

            if (_inputSystem.RotationValue == 0)
                return;
            UpdateRotation();
        }
        void IUpdatable.Clear()
        {
            OnShipDestroy.RemoveAllListeners();
            _model.Clear();
        }

        public ShipModel GetShipModel() => _model;

        private void onShipCollision()
        {
            OnShipDestroy?.Invoke();
            _model.OnCollision.RemoveListener(onShipCollision);
        }

        private void UpdateAcceleration()
        {
            if (_inputSystem.MoveForwardPhase == UnityEngine.InputSystem.InputActionPhase.Performed)
                _shipTransformHandler.IncreaseAcceleration(_model.Rotation, Time.deltaTime);
            else
                _shipTransformHandler.DecreaseAcceleration(Time.deltaTime);
        }
        private void UpdatePosition()
        {
            _shipTransformHandler.ChangePosition(_model);
            _view.ChangePosition(_model.Position);
        }

        private void UpdateRotation()
        {
            _shipTransformHandler.ChangeRotation(_model, _inputSystem.RotationValue, Time.deltaTime);
            _view.ChangeRotation(_model.Rotation);
        }
    }
}