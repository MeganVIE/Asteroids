using UnityEngine;

public class ModelTransform
{
    public Vector2 Position { get; private set; }
    public float Rotation { get; private set; }

    public ModelTransform(Vector2 startPosition, float startRotation = 0)
    {
        Position = startPosition;
        Rotation = startRotation;
    }

    public void ChangePosition(Vector2 newPosition) => Position = newPosition;
    public void ChangeRotation(float newRotation) => Rotation = newRotation;
}