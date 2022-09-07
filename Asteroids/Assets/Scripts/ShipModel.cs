using UnityEngine;

public class ShipModel
{
    private float _maxSpeed = 0.001f;
    private float _accelerationSpeed = 0.0005f;
    private float _slowdownSpeed = 1f;
    private float _rotationSpeed = 90;

    private Vector2 _acceleration;
    private Vector2 _position;

    public Vector2 Position => _position;
    public float Rotation { get; private set; }

    public ShipModel(Vector2 startPosition)
    {
        _position = startPosition;
    }

    public void IncreaseAcceleration(float deltaTime)
    {
        Vector2 forwardDirection = Quaternion.Euler(0, 0, Rotation) * Vector3.up;
        _acceleration += forwardDirection * (_accelerationSpeed * deltaTime);
        _acceleration = Vector2.ClampMagnitude(_acceleration, _maxSpeed);
    }
    public void DecreaseAcceleration(float deltaTime)
    {
        _acceleration -= _acceleration * (deltaTime / _slowdownSpeed);
    }

    public void ChangeRotation(float angleDirection, float deltaTime)
    {
        Rotation = Mathf.Repeat(Rotation + angleDirection * deltaTime * _rotationSpeed, 360);
    }

    public void ChangePosition()
    {
        _position += _acceleration;

        _position.x = Mathf.Repeat(_position.x, 1);
        _position.y = Mathf.Repeat(_position.y, 1);
    }
}
