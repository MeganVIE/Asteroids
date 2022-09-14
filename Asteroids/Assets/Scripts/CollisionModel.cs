using System;

public class CollisionModel : Model
{
    public float CollisionRadius { get; private set; }
    public ObjectType CollisionType { get; protected set; }
    public Action OnCollision { get; set; }

    public void SetCollisionRadius(float radius) => CollisionRadius = radius;
}

public enum ObjectType
{
    Ship,
    Bullet,
    Enemy
}