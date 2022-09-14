using System;
using UnityEngine;

namespace Weapon
{
    public class BulletModel : ICollision
    {
        private float _collideRadius;
        float ICollision.Radius => _collideRadius;
        ObjectType ICollision.CollisionType => ObjectType.Bullet;

        public Vector2 Direction { get; private set; }
        public Vector2 Position { get; private set; }
        public Action OnCollision { get; set; }

        public void ChangePosition(Vector2 newPosition) => Position = newPosition;
        public void ChangeDirection(Vector2 newDirection) => Direction = newDirection;

        public void SetCollisionRadius(float radius) => _collideRadius = radius;

        public Vector2 GetPosition() => Position;
    }
}
