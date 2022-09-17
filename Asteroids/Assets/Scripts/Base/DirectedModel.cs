using UnityEngine;

public abstract class DirectedModel : CollisionModel
{
    public Vector2 Direction { get; private set; }
    public void ChangeDirection(Vector2 newDirection) => Direction = newDirection;
}