using UnityEngine;

namespace Ship
{
    public class ShipController
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
            _view.SetModel(_model);            
        }

        public void Update()
        {
            if (_inputSystem.MoveForwardPhase == UnityEngine.InputSystem.InputActionPhase.Performed)
                _shipTransformHandler.IncreaseAcceleration(_model.Rotation, Time.deltaTime);
            else
                _shipTransformHandler.DecreaseAcceleration(Time.deltaTime);

            _shipTransformHandler.ChangePosition(_model);
            _view.UpdatePosition();

            if (_inputSystem.RotationValue == 0)
                return;

            _shipTransformHandler.ChangeRotation(_model, _inputSystem.RotationValue, Time.deltaTime);            
            _view.UpdateRotation();
        }
    }
}