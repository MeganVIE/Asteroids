using System;
using UnityEngine;

namespace Enemies
{
    public class AsteroidModel : ICollision
    {
        private float _collisionRadius;
        float ICollision.Radius => _collisionRadius;
        ObjectType ICollision.CollisionType => ObjectType.Enemy;
        public Vector2 Direction { get; private set; }
        public Vector2 Position { get; private set; }
        public Action OnCollision { get; set; }

        public void ChangePosition(Vector2 newPosition) => Position = newPosition;
        public void ChangeDirection(Vector2 newDirection) => Direction = newDirection;
        public void SetCollisionRadius(float radius) => _collisionRadius = radius;

        public Vector2 GetPosition() => Position;
    }
}
