using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler
{
    private Dictionary<ObjectType, ObjectType> _collisionsMap;
    private Dictionary<ObjectType, List<ICollision>> _collisionObjects;

    public CollisionHandler()
    {
        _collisionObjects = new Dictionary<ObjectType, List<ICollision>>();
        _collisionsMap = new Dictionary<ObjectType, ObjectType>()
        {
            [ObjectType.Ship] = ObjectType.Enemy,
            [ObjectType.Bullet] = ObjectType.Enemy
        };
    }

    public void AddCollision(ICollision collision)
    {
        if (_collisionObjects.ContainsKey(collision.CollisionType))
            _collisionObjects[collision.CollisionType].Add(collision);
        else
            _collisionObjects.Add(collision.CollisionType, new List<ICollision>() { collision });
    }

    public void RemoveCollision(ICollision collision)
    {
        if(_collisionObjects.ContainsKey(collision.CollisionType))
            _collisionObjects[collision.CollisionType].Remove(collision);
    }

    public void Update()
    {
        foreach (var pair in _collisionsMap)
        {
            if (!_collisionObjects.ContainsKey(pair.Key))
                continue;
            foreach (var mainCollision in _collisionObjects[pair.Key])
            {
                if (!_collisionObjects.ContainsKey(pair.Value))
                    continue;
                foreach (var targetCollision in _collisionObjects[pair.Value])
                {
                    if ((mainCollision.GetPosition() - targetCollision.GetPosition()).magnitude < mainCollision.Radius + targetCollision.Radius)
                        mainCollision.OnCollision.Invoke();
                }
            }
        }
    }
}