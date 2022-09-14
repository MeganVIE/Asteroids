using System;
using UnityEngine;

namespace Ship
{
    public class ShipModel : ICollision
    {
        private float _collisionRadius;
        public ModelTransform modelTransform { get; private set; }
        float ICollision.Radius => _collisionRadius;
        ObjectType ICollision.CollisionType => ObjectType.Ship;

        public Action OnCollision { get; set; }

        public ShipModel(Vector2 startPosition, float collisionRadius)
        {
            modelTransform = new ModelTransform(startPosition);
            _collisionRadius = collisionRadius;
        }

        public Vector2 GetPosition() => modelTransform.Position;
    }
}