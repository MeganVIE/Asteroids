using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T1,T2> where T1 : class, new() where T2 : MonoBehaviour
{
    private T2 _prefab;
    private Dictionary<T1, T2> _unusedObjects;

    public ObjectPool(T2 prefab)
    {
        _prefab = prefab;
        _unusedObjects = new Dictionary<T1, T2>();
    }

    public void GetObject(out T1 model, out T2 view)
    {
        if (_unusedObjects.Count == 0)
        {
            model = new T1();
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

    public void DeactivateObject(T1 model, T2 view)
    {
        _unusedObjects.Add(model, view);
        view.gameObject.SetActive(false);
    }
}