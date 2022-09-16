using UnityEngine;

namespace Ship
{
    public class ShipModel : CollisionModel
    {
        public float Rotation { get; private set; }

        public ShipModel(Vector2 startPosition, float collisionRadius)
        {
            ChangePosition(startPosition);
            SetCollisionData(collisionRadius, ObjectType.Ship);
        }

        public void ChangeRotation(float newRotation) => Rotation = newRotation;

        public override bool Collision(CollisionModel targetModel)
        {
            return (Position - targetModel.Position).magnitude < CollisionRadius + targetModel.CollisionRadius;
        }
    }
}