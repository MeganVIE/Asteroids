using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Enemies
{
    public class AsteroidController : IController
    {
        private AsteroidConfig _asteroidConfig;
        private ObjectPool<AsteroidModel, AsteroidView> _asteroidObjectPool;
        private Dictionary<AsteroidModel, AsteroidView> _asteroids;

        private float _currentSpawnTime;
        private float _timer;

        public AsteroidController(AsteroidConfig asteroidConfig)
        {
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
            model.ChangePosition(GetRandomPosition());
            model.ChangeDirection((new Vector2(Random.value, Random.value) - model.Position).normalized);
            view.ChangePosition(model.Position);
            _asteroids.Add(model, view);
        }

        private Vector2 GetRandomPosition()
        {
            var position = Random.insideUnitCircle;
            if (Random.Range(0, 2) == 0)
                position.x = position.x > .5f ? 1 : 0;
            else
                position.y = position.y > .5f ? 1 : 0;
            return position;
        }

        private void UpdatePositions()
        {
            foreach (var pair in _asteroids)
            {
                var newPosition = pair.Key.Position + pair.Key.Direction * _asteroidConfig.AsteroidSpeed * Time.deltaTime;
                newPosition.x = Mathf.Repeat(newPosition.x, 1);
                newPosition.y = Mathf.Repeat(newPosition.y, 1);

                pair.Key.ChangePosition(newPosition);
                _asteroids[pair.Key].ChangePosition(newPosition);
            }
        }
    }
}
