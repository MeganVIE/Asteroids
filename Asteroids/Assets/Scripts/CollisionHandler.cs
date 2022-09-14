using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler
{
    private Dictionary<ObjectType, ObjectType> _collisionsMap;
    private Dictionary<ObjectType, List<ICollision>> _collisionObjects;

    private Camera _camera;

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
        if (_camera == null)
            _camera = Camera.main;

        foreach (var pair in _collisionsMap)
        {
            foreach (var mainCollision in _collisionObjects[pair.Key])
            {
                if (!_collisionObjects.ContainsKey(pair.Value))
                    continue;
                var mainPosition = _camera.ViewportToWorldPoint(mainCollision.GetPosition());
                foreach (var targetCollision in _collisionObjects[pair.Value])
                {
                    var targetPosition = _camera.ViewportToWorldPoint(targetCollision.GetPosition());
                    if ((mainPosition - targetPosition).magnitude < mainCollision.Radius + targetCollision.Radius)
                        mainCollision.OnCollision?.Invoke();                        
                }
            }
        }
    }
}