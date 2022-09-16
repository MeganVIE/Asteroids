using System;

public abstract class CollisionModel : Model
{
    public float CollisionRadius { get; private set; }
    public ObjectType ObjectCollisionType { get; private set; }
    public Action OnCollision { get; set; }

    public void SetCollisionData(float radius, ObjectType type)
    {
        CollisionRadius = radius;
        ObjectCollisionType = type;
    }
    public abstract bool Collision(CollisionModel targetModel);
}

public enum ObjectType
{
    Ship,
    Bullet,
    Laser,
    Enemy
}