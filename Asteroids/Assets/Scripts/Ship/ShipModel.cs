using UnityEngine;

namespace Ship
{
    public class ShipModel : CollisionModel
    {
        public float Rotation { get; private set; }

        public ShipModel(Vector2 startPosition, float collisionRadius)
        {
            ChangePosition(startPosition);
            SetCollisionRadius(collisionRadius);
            CollisionType = ObjectType.Ship;
        }

        public void ChangeRotation(float newRotation) => Rotation = newRotation;
    }
}