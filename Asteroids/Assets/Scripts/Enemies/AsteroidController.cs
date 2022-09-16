using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class AsteroidController : IUpdatable
    {
        private AsteroidConfig _asteroidConfig;
        private CollisionHandler _collisionHandler;
        private ObjectPool<DestroyableDirectedModel, BigAsteroidView> _bigAsteroidObjectPool;
        private Dictionary<DestroyableDirectedModel, BigAsteroidView> _bigAsteroids;

        private float _currentSpawnTime;
        private float _timer;

        public AsteroidController(AsteroidConfig asteroidConfig, CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _asteroidConfig = asteroidConfig;
            _bigAsteroidObjectPool = new ObjectPool<DestroyableDirectedModel, BigAsteroidView>(_asteroidConfig.BigAsteroidViewPrefab, ObjectType.Enemy,_asteroidConfig.CollisionRadius);
            _bigAsteroids = new Dictionary<DestroyableDirectedModel, BigAsteroidView>();

            _currentSpawnTime = _asteroidConfig.AsteroidFirstSpawnTime;
            _timer = 0;
        }

        void IUpdatable.Update()
        {
            Timer();

            if (_bigAsteroids.Count == 0)
                return;

            UpdatePositions();
        }

        private void Timer()
        {
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnTime)
            {
                _timer = 0;
                _currentSpawnTime = _asteroidConfig.AsteroidDelaySpawnTime;
                SpawnAsteroid();
            }
        }

        private void SpawnAsteroid()
        {
            _bigAsteroidObjectPool.GetModelViewPair(out DestroyableDirectedModel model, out BigAsteroidView view);
            model.ChangePosition(CameraData.GetRandomPositionOnBound());
            model.ChangeDirection(new Vector2(Random.value, Random.value) - model.Position);
            view.ChangePosition(model.Position);
            _bigAsteroids.Add(model, view);
            _collisionHandler.AddCollision(model);

            model.OnDestroy += DeactivateAsteroid;
        }

        private void UpdatePositions()
        {
            foreach (var pair in _bigAsteroids)
            {
                var newPosition = pair.Key.Position + pair.Key.Direction * _asteroidConfig.AsteroidSpeed * Time.deltaTime;
                newPosition = CameraData.RepeatInViewport(newPosition);

                pair.Key.ChangePosition(newPosition);
                _bigAsteroids[pair.Key].ChangePosition(newPosition);
            }
        }

        private void DeactivateAsteroid(DestroyableDirectedModel model)
        {
            _bigAsteroidObjectPool.DeactivateModelViewPair(model, _bigAsteroids[model]);
            _bigAsteroids.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateAsteroid;
        }
    }
}
