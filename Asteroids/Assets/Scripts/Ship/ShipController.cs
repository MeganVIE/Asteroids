using System;
using UnityEngine;

namespace Ship
{
    public class ShipController : IUpdatable, IClearable, IRestartable
    {
        private IMoveRotateInputData _inputSystem;

        private ShipModel _model;
        private ShipView _view;
        private ShipTransformHandler _shipTransformHandler;
        private CollisionHandler _collisionHandler;
        private ShipConfig _shipConfig;

        public Action OnShipDestroy { get; set; }

        public Action<Vector2> OnCoordinatesChange { get; set; }
        public Action<float> OnRotationChange { get; set; }
        public Action<float> OnSpeedChange { get; set; }

        public ShipController(ShipConfig shipConfig, IMoveRotateInputData inputs, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _shipConfig = shipConfig;
            _shipTransformHandler = new ShipTransformHandler(_shipConfig, cameraData);
            _inputSystem = inputs;            

            _model = new ShipModel(_shipConfig.StartPosition, _shipConfig.CollisionRadius);
            _view = UnityEngine.Object.Instantiate(_shipConfig.ViewPrefab);
            _collisionHandler = collisionHandler;
            _collisionHandler.AddCollision(_model);

            Restart();
        }

        public void Restart()
        {
            _model.ChangePosition(_shipConfig.StartPosition);
            _model.ChangeRotation(_shipConfig.StartRotation);
            _shipTransformHandler.Restart();

            UpdatePosition();
            UpdateRotation();

            _model.OnCollision += onShipCollision;
        }

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
            _collisionHandler.RemoveCollision(_model);
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

            OnSpeedChange?.Invoke(_shipTransformHandler.CurrentSpeed);
        }
        private void UpdatePosition()
        {
            _shipTransformHandler.ChangePosition(_model);
            _view.ChangePosition(_model.Position);

            OnCoordinatesChange?.Invoke(_model.Position);
        }

        private void UpdateRotation()
        {
            _shipTransformHandler.ChangeRotation(_model, _inputSystem.RotationValue, Time.deltaTime);
            _view.ChangeRotation(_model.Rotation);

            OnRotationChange?.Invoke(_model.Rotation);
        }
    }
}