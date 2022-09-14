using UnityEngine;

namespace Ship
{
    public class ShipTransformHandler
    {
        private ShipConfig _shipConfigData;
        private Vector2 _acceleration;

        public ShipTransformHandler(ShipConfig shipConfigData)
        {
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

        public void ChangeRotation(ModelTransform modelTransform, float angleDirection, float deltaTime)
        {
            var newRotation = Mathf.Repeat(modelTransform.Rotation + angleDirection * deltaTime * _shipConfigData.RotationSpeed, 360);
            modelTransform.ChangeRotation(newRotation);
        }

        public void ChangePosition(ModelTransform modelTransform)
        {
            var newPosition = modelTransform.Position + _acceleration;
            newPosition = CameraData.RepeatInViewport(newPosition);

            modelTransform.ChangePosition(newPosition);
        }
    }
}
