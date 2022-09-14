using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler
{
    private Dictionary<ObjectType, ObjectType> _collisionsMap;
    private Dictionary<ObjectType, List<CollisionModel>> _collisionObjects;
    public CollisionHandler()
    {
        _collisionObjects = new Dictionary<ObjectType, List<CollisionModel>>();
        _collisionsMap = new Dictionary<ObjectType, ObjectType>()
        {
            [ObjectType.Ship] = ObjectType.Enemy,
            [ObjectType.Bullet] = ObjectType.Enemy
        };
    }

    public void AddCollision(CollisionModel model)
    {
        if (_collisionObjects.ContainsKey(model.CollisionType))
            _collisionObjects[model.CollisionType].Add(model);
        else
            _collisionObjects.Add(model.CollisionType, new List<CollisionModel>() { model });
    }

    public void RemoveCollision(CollisionModel model)
    {
        if (_collisionObjects.ContainsKey(model.CollisionType))
            _collisionObjects[model.CollisionType].Remove(model);
    }

    public void Update()
    {
        foreach (var pair in _collisionsMap)
        {
            if (!_collisionObjects.ContainsKey(pair.Key))
                continue;
            var mains = _collisionObjects[pair.Key];

            for (int j = 0; j < mains.Count; j++)
            {
                if (!_collisionObjects.ContainsKey(pair.Value))
                    continue;
                var targets = _collisionObjects[pair.Value];

                for (int i = 0; j>=0 && i < targets.Count; i++)
                {
                    if ((mains[j].Position - targets[i].Position).magnitude < mains[j].CollisionRadius + targets[i].CollisionRadius)
                    {
                        mains[j--].OnCollision?.Invoke();
                        targets[i--].OnCollision?.Invoke();
                    }
                }
            }
        }
    }
}