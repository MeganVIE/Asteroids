using UnityEngine;

public class Model
{
    public Vector2 Position { get; private set; }
    public void ChangePosition(Vector2 newPosition) => Position = newPosition;
}