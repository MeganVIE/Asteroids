using System;

public class DestroyableDirectedModel : DirectedModel
{
    public Action<DestroyableDirectedModel> OnDestroy { get; set; }

    public DestroyableDirectedModel()
    {
        OnCollision += CollisionHandler;
    }
    public override bool HasCollision(CollisionModel targetModel)
    {
        return (Position - targetModel.Position).magnitude < CollisionRadius + targetModel.CollisionRadius;
    }

    public void Clear()
    {
        OnCollision -= CollisionHandler;
    }

    private void CollisionHandler()
    {
        OnDestroy?.Invoke(this);
    }    
}