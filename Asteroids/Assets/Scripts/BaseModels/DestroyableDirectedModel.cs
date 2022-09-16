using System;

public class DestroyableDirectedModel : DirectedModel
{
    public Action<DestroyableDirectedModel> OnDestroy { get; set; }

    public DestroyableDirectedModel()
    {
        OnCollision += CollisionHandler;
    }
    public override bool Collision(CollisionModel targetModel)
    {
        return (Position - targetModel.Position).magnitude < CollisionRadius + targetModel.CollisionRadius;
    }

    private void CollisionHandler()
    {
        OnDestroy?.Invoke(this);
    }
}