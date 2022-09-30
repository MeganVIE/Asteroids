using UnityEngine;

public class DirectedModelTransformHandler
{
    public Vector2 GetNewPosition(DestroyableDirectedModel model, float speed)
    {
        return model.Position + model.Direction * speed * Time.deltaTime;
    }
}