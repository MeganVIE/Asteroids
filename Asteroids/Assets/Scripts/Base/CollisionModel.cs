using UnityEngine;
using UnityEngine.Events;

public abstract class CollisionModel
{
    public Vector2 Position { get; private set; }
    public float CollisionRadius { get; private set; }
    public ObjectType ObjectCollisionType { get; private set; }
    public UnityEvent OnCollision { get; private set; }

    public CollisionModel()
    {
        OnCollision = new UnityEvent();
    }

    public void ChangePosition(Vector2 newPosition) => Position = newPosition;
    public void SetCollisionData(float radius, ObjectType type)
    {
        CollisionRadius = radius;
        ObjectCollisionType = type;
    }
    public abstract bool Collision(CollisionModel targetModel);

    public virtual void Clear()
    {
        OnCollision.RemoveAllListeners();
    }
}

public enum ObjectType
{
    Ship,
    Bullet,
    Laser,
    Enemy
}