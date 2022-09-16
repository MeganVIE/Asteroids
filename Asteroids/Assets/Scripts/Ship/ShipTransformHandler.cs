using UnityEngine;

namespace Ship
{
    public class ShipTransformHandler
    {
        private CameraData _cameraData;
        private ShipConfig _shipConfigData;
        private Vector2 _acceleration;

        public ShipTransformHandler(ShipConfig shipConfigData, CameraData cameraData)
        {
            _cameraData = cameraData;
            _shipConfigData = shipConfigData;
        }

        public void IncreaseAcceleration(float modelRotation, float deltaTime)
        {
            Vector2 forwardDirection = Quaternion.Euler(0, 0, modelRotation) * Vector3.up;
            _acceleration += forwardDirection * (_shipConfigData.AccelerationSpeed * deltaTime);
            _acceleration = Vector2.ClampMagnitude(_acceleration, _shipConfigData.MaxSpeed);
        }
        public void DecreaseAcceleration(float deltaTime)
        {
            _acceleration -= _acceleration * (deltaTime / _shipConfigData.SlowdownSpeed);
        }

        public void ChangeRotation(ShipModel model, float angleDirection, float deltaTime)
        {
            var newRotation = Mathf.Repeat(model.Rotation + angleDirection * deltaTime * _shipConfigData.RotationSpeed, 360);
            model.ChangeRotation(newRotation);
        }

        public void ChangePosition(ShipModel model)
        {
            var newPosition = model.Position + _acceleration;
            newPosition = _cameraData.RepeatInViewport(newPosition);

            model.ChangePosition(newPosition);
        }
    }
}
