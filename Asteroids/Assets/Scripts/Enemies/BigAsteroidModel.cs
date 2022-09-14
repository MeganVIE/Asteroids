using System;
using UnityEngine;

namespace Enemies
{
    public class BigAsteroidModel : CollisionModel
    {
        public Action<BigAsteroidModel> OnDestroy { get; set; }
        public Vector2 Direction { get; private set; }

        public BigAsteroidModel()
        {
            CollisionType = ObjectType.Enemy;
            OnCollision += CollisionHandler;
        }
        public void ChangeDirection(Vector2 newDirection) => Direction = newDirection;

        private void CollisionHandler()
        {
            OnDestroy?.Invoke(this);
        }
    }
}
