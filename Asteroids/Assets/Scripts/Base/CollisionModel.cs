using System;
using UnityEngine;

public abstract class CollisionModel
{
    public Vector2 Position { get; private set; }
    public float CollisionRadius { get; private set; }
    public ObjectType ObjectCollisionType { get; private set; }
    public Action OnCollision { get; set; }

    public void ChangePosition(Vector2 newPosition) => Position = newPosition;
    public void SetCollisionData(float radius, ObjectType type)
    {
        CollisionRadius = radius;
        ObjectCollisionType = type;
    }
    public abstract bool HasCollision(CollisionModel targetModel);
}

public enum ObjectType
{
    Ship,
    Bullet,
    Laser,
    Enemy
}