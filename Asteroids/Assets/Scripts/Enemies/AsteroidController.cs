using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class AsteroidController : IController
    {
        private AsteroidConfig _asteroidConfig;
        private CollisionHandler _collisionHandler;
        private ObjectPool<BigAsteroidModel, BigAsteroidView> _bigAsteroidObjectPool;
        private Dictionary<BigAsteroidModel, BigAsteroidView> _bigAsteroids;

        private float _currentSpawnTime;
        private float _timer;

        public AsteroidController(AsteroidConfig asteroidConfig, CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _asteroidConfig = asteroidConfig;
            _bigAsteroidObjectPool = new ObjectPool<BigAsteroidModel, BigAsteroidView>(_asteroidConfig.BigAsteroidViewPrefab);
            _bigAsteroids = new Dictionary<BigAsteroidModel, BigAsteroidView>();

            _currentSpawnTime = _asteroidConfig.AsteroidFirstSpawnTime;
            _timer = 0;
        }

        void IController.Update()
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
            _bigAsteroidObjectPool.GetObject(out BigAsteroidModel model, out BigAsteroidView view);
            model.ChangePosition(CameraData.GetRandomPositionOnBound());
            model.ChangeDirection(new Vector2(Random.value, Random.value) - model.Position);
            model.SetCollisionRadius(_asteroidConfig.CollisionRadius);
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

        private void DeactivateAsteroid(BigAsteroidModel model)
        {
            _bigAsteroidObjectPool.DeactivateObject(model, _bigAsteroids[model]);
            _bigAsteroids.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateAsteroid;
        }
    }
}
