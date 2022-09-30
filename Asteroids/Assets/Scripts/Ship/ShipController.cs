using UnityEngine;

namespace Ship
{
    public class ShipController : IUpdatable, IClearable
    {
        private IMoveRotateInputData _inputSystem;

        private ShipModel _model;
        private ShipView _view;
        private ShipTransformHandler _shipTransformHandler;
        private CollisionHandler _collisionHandler;

        public System.Action OnShipDestroy { get; set; }

        public ShipController(ShipConfig shipConfig, IMoveRotateInputData inputs, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _shipTransformHandler = new ShipTransformHandler(shipConfig, cameraData);
            _inputSystem = inputs;

            _model = new ShipModel(shipConfig.StartPosition, shipConfig.CollisionRadius);
            _view = Object.Instantiate(shipConfig.ViewPrefab);
            _collisionHandler = collisionHandler;
            _collisionHandler.AddCollision(_model);

            _model.OnCollision += onShipCollision;

            UpdatePosition();
            UpdateRotation();
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
        void IClearable.Clear()
        {
            _model.OnCollision -= onShipCollision;
        }

        public ShipModel GetShipModel() => _model;

        private void onShipCollision()
        {
            OnShipDestroy?.Invoke();
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