using UnityEngine;

namespace Weapon
{
    public class LaserModel : DirectedModel
    {
        public override bool HasCollision(CollisionModel targetModel)
        {
            var projectPoint = Position + Direction * Vector2.Dot(targetModel.Position - Position, Direction) / Vector2.Dot(Direction, Direction);
            var perpendicularSqrLength = (projectPoint - targetModel.Position).sqrMagnitude;
            var projectVector = projectPoint - Position;

            return perpendicularSqrLength < targetModel.CollisionRadius * targetModel.CollisionRadius
                && Vector2.Dot(projectVector, Direction) > 0
                && projectVector.sqrMagnitude < CollisionRadius * CollisionRadius;
        }
    }
}
