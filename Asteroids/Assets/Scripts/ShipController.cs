using UnityEngine;

public class ShipController
{
    private InputSystem _inputSystem;

    private ShipModel _model;
    private ShipView _view;

    private Vector2 _startPosition = new Vector2(.5f, .5f);

    public ShipController(ShipView view, InputSystem inputs)
    {
        _model = new ShipModel(_startPosition);
        _view = view;
        _view.SetModel(_model);

        _inputSystem = inputs;
    }

    public void Update()
    {
        if (_inputSystem.MoveForwardPhase == UnityEngine.InputSystem.InputActionPhase.Performed)
            _model.IncreaseAcceleration(Time.deltaTime);
        else
            _model.DecreaseAcceleration(Time.deltaTime);

        _model.ChangePosition();
        _view.UpdatePosition();

        _model.ChangeRotation(_inputSystem.RotationValue, Time.deltaTime);
        _view.UpdateRotation();
    }
}
