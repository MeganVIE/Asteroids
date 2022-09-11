using UnityEngine;

namespace Ship
{
    public class ShipModel
    {
        private Vector2 _position;
        public Vector2 Position => _position;
        public float Rotation { get; private set; }

        public ShipModel(Vector2 startPosition)
        {
            _position = startPosition;
        }

        public void ChangePosition(Vector2 newPosition) => _position = newPosition;
        public void ChangeRotation(float newRotation) => Rotation = newRotation;
    }
}