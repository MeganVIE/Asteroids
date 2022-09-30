using System.Collections.Generic;

public class CollisionHandler : IUpdatable, IClearable
{
    private Dictionary<ObjectType, ObjectType> _collisionsMap;
    private Dictionary<ObjectType, List<CollisionModel>> _collisionObjects;
    public CollisionHandler()
    {
        _collisionObjects = new Dictionary<ObjectType, List<CollisionModel>>();
        _collisionsMap = new Dictionary<ObjectType, ObjectType>()
        {
            [ObjectType.Ship] = ObjectType.Enemy,
            [ObjectType.Bullet] = ObjectType.Enemy,
            [ObjectType.Laser] = ObjectType.Enemy
        };
    }

    public void AddCollision(CollisionModel model)
    {
        if (_collisionObjects.ContainsKey(model.ObjectCollisionType))
            _collisionObjects[model.ObjectCollisionType].Add(model);
        else
            _collisionObjects.Add(model.ObjectCollisionType, new List<CollisionModel>() { model });
    }

    public void RemoveCollision(CollisionModel model)
    {
        if (_collisionObjects.ContainsKey(model.ObjectCollisionType))
            _collisionObjects[model.ObjectCollisionType].Remove(model);
    }

    void IUpdatable.Update()
    {
        CheckCollisions();
    }
    void IClearable.Clear()
    {
        _collisionsMap.Clear();
        _collisionObjects.Clear();
    }

    private void CheckCollisions()
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

                for (int i = 0; j >= 0 && i < targets.Count; i++)
                {
                    if (mains[j].HasCollision(targets[i]))
                    {
                        var main = mains[j];
                        main.OnCollision?.Invoke();
                        targets[i--].OnCollision?.Invoke();

                        if (main.ObjectCollisionType != ObjectType.Laser)
                            j--;
                    }
                }
            }
        }
    }
}