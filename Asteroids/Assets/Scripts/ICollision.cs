using System;
using UnityEngine;

public interface ICollision
{
    float Radius { get; }
    ObjectType CollisionType { get; }
    Vector2 GetPosition();
    Action OnCollision { get; set; }
}

public enum ObjectType
{
    Ship,
    Bullet,
    Enemy
}