using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T1,T2> where T1 : CollisionModel, new() where T2 : View
{
    private T2 _prefab;
    private ObjectType _type;
    private float _radius;
    private Dictionary<T1, T2> _unusedObjects;

    public ObjectPool(T2 prefab, ObjectType type, float radius)
    {
        _prefab = prefab;
        _type = type;
        _radius = radius;
        _unusedObjects = new Dictionary<T1, T2>();
    }

    public void Clear()
    {
        foreach (var pair in _unusedObjects)
        {
            OnPairClear(pair);
            //pair.Key.Clear();
            //Object.Destroy(pair.Value);
        }
        _unusedObjects.Clear();
    }

    public void GetModelViewPair(out T1 model, out T2 view)
    {
        if (_unusedObjects.Count == 0)
        {
            model = new T1();
            model.SetCollisionData(_radius, _type);
            view = Object.Instantiate(_prefab);
        }
        else
        {
            model = _unusedObjects.Keys.First();
            view = _unusedObjects[model];
            view.gameObject.SetActive(true);
            _unusedObjects.Remove(model);
        }
    }

    public void DeactivateModelViewPair(T1 model, T2 view)
    {
        _unusedObjects.Add(model, view);
        view.gameObject.SetActive(false);
    }

    protected virtual void OnPairClear(KeyValuePair<T1, T2> pair)
    {
        Object.Destroy(pair.Value);
    }
}