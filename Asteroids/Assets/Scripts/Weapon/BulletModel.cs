﻿using System;
using UnityEngine;

namespace Weapon
{
    public class BulletModel : CollisionModel
    {
        public Action<BulletModel> OnDestroy { get; set; }
        public Vector2 Direction { get; private set; }

        public BulletModel()
        {
            CollisionType = ObjectType.Bullet;
            OnCollision += CollisionHandler;
        }

        public void ChangeDirection(Vector2 newDirection) => Direction = newDirection;

        private void CollisionHandler()
        {
            OnDestroy?.Invoke(this);
        }
    }
}
