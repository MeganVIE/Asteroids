using UnityEngine;

namespace Ship
{
    public class ShipController : IController
    {
        private IMoveRotateInputData _inputSystem;

        private ShipModel _model;
        private ShipView _view;
        private ShipTransformHandler _shipTransformHandler;

        public ShipController(ShipConfig shipConfigData, ShipView view, IMoveRotateInputData inputs)
        {
            _shipTransformHandler = new ShipTransformHandler(shipConfigData);
            _inputSystem = inputs;

            _model = new ShipModel(shipConfigData.StartPosition);
            _view = view;      
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