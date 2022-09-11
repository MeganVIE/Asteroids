using UnityEngine;

namespace Weapon
{
    public class BulletModel
    {
        public Vector2 Direction { get; private set; }
        public Vector2 Position { get; private set; }

        public void ChangePosition(Vector2 newPosition) => Position = newPosition;
        public void ChangeDirection(Vector2 newDirection) => Direction = newDirection;
    }
}
