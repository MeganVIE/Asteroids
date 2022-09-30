using System.Collections.Generic;

public class DestroyableObjectPool<T1, T2> : ObjectPool<T1, T2> where T1 : DestroyableDirectedModel, new() where T2 : View
{
    public DestroyableObjectPool(T2 prefab, ObjectType type, float radius) : base(prefab, type, radius) { }

    protected override void OnPairClear(KeyValuePair<T1, T2> pair)
    {
        base.OnPairClear(pair);
        pair.Key.Clear();
    }
}
