using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class AsteroidController : IController
    {
        private AsteroidConfig _asteroidConfig;
        private CollisionHandler _collisionHandler;
        private ObjectPool<AsteroidModel, AsteroidView> _asteroidObjectPool;
        private Dictionary<AsteroidModel, AsteroidView> _asteroids;

        private float _currentSpawnTime;
        private float _timer;

        public AsteroidController(AsteroidConfig asteroidConfig, CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _asteroidConfig = asteroidConfig;
            _asteroidObjectPool = new ObjectPool<AsteroidModel, AsteroidView>(_asteroidConfig.AsteroidViewPrefab);
            _asteroids = new Dictionary<AsteroidModel, AsteroidView>();

            _currentSpawnTime = _asteroidConfig.AsteroidFirstSpawnTime;
            _timer = 0;
        }

        void IController.Update()
        {
            Timer();

            if (_asteroids.Count == 0)
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
            _asteroidObjectPool.GetObject(out AsteroidModel model, out AsteroidView view);
            model.ChangePosition(CameraData.GetRandomPositionOnBound());
            model.ChangeDirection((new Vector2(Random.value, Random.value) - model.Position)/*.normalized*/);
            model.SetCollisionRadius(_asteroidConfig.CollisionRadius);
            view.ChangePosition(model.Position);
            _asteroids.Add(model, view);
            _collisionHandler.AddCollision(model);
        }

        private void UpdatePositions()
        {
            foreach (var pair in _asteroids)
            {
                var newPosition = pair.Key.Position + pair.Key.Direction * _asteroidConfig.AsteroidSpeed * Time.deltaTime;
                newPosition = CameraData.RepeatInViewport(newPosition);

                pair.Key.ChangePosition(newPosition);
                _asteroids[pair.Key].ChangePosition(newPosition);
            }
        }

        private void DeactivateAsteroid()
        {

        }
    }
}
