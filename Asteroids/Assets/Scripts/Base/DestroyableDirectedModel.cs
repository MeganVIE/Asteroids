using UnityEngine.Events;

public class DestroyableDirectedModel : DirectedModel
{
    public UnityEvent<DestroyableDirectedModel> OnDestroy { get; set; }

    public DestroyableDirectedModel()
    {
        OnCollision.AddListener(CollisionHandler);
        OnDestroy = new UnityEvent<DestroyableDirectedModel>();
    }
    public override bool HasCollision(CollisionModel targetModel)
    {
        return (Position - targetModel.Position).magnitude < CollisionRadius + targetModel.CollisionRadius;
    }

    private void CollisionHandler()
    {
        OnDestroy?.Invoke(this);
    }

    public override void Clear()
    {
        base.Clear();
        OnDestroy.RemoveAllListeners();
    }
}

public class Enemy : DestroyableDirectedModel
{
    public int Points { get; private set; }
    public void SetPoints(int points) => Points = points;
}