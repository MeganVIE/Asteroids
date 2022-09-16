using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class AsteroidController : IUpdatable
    {
        private AsteroidConfig _asteroidConfig;
        private CollisionHandler _collisionHandler;
        private ObjectPool<DestroyableDirectedModel, AsteroidView> _bigAsteroidsObjectPool;
        private ObjectPool<DestroyableDirectedModel, AsteroidView> _smallAsteroidsObjectPool;
        private Dictionary<DestroyableDirectedModel, AsteroidView> _bigAsteroids;
        private Dictionary<DestroyableDirectedModel, AsteroidView> _smallAsteroids;

        private float _currentSpawnTime;
        private float _timer;

        public AsteroidController(AsteroidConfig asteroidConfig, CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _asteroidConfig = asteroidConfig;
            _bigAsteroidsObjectPool = new ObjectPool<DestroyableDirectedModel, AsteroidView>(_asteroidConfig.BigAsteroidViewPrefab, ObjectType.Enemy,_asteroidConfig.BigAsteroidCollisionRadius);
            _smallAsteroidsObjectPool = new ObjectPool<DestroyableDirectedModel, AsteroidView>(_asteroidConfig.SmallAsteroidViewPrefab, ObjectType.Enemy, _asteroidConfig.SmallAsteroidCollisionRadius);
            _bigAsteroids = new Dictionary<DestroyableDirectedModel, AsteroidView>();
            _smallAsteroids = new Dictionary<DestroyableDirectedModel, AsteroidView>();

            _currentSpawnTime = _asteroidConfig.AsteroidFirstSpawnTime;
            _timer = 0;
        }

        void IUpdatable.Update()
        {
            Timer();

            if (_bigAsteroids.Count > 0)
                UpdatePositions(_bigAsteroids, _asteroidConfig.BigAsteroidSpeed);

            if (_smallAsteroids.Count > 0)
                UpdatePositions(_smallAsteroids, _asteroidConfig.SmallAsteroidSpeed);
        }

        private void Timer()
        {
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnTime)
            {
                _timer = 0;
                _currentSpawnTime = _asteroidConfig.AsteroidDelaySpawnTime;
                SpawnBigAsteroid();
            }
        }

        private void SpawnBigAsteroid()
        {
            _bigAsteroidsObjectPool.GetModelViewPair(out DestroyableDirectedModel model, out AsteroidView view);
            AsteroidModelViewSettings(model, view, CameraData.GetRandomPositionOnBound());
            _bigAsteroids.Add(model, view);
            model.OnDestroy += DeactivateBigAsteroid;
        }

        private void SpawnSmallAsteroids(Vector2 position)
        {
            for (int i = 0; i < _asteroidConfig.SmallAsteroidSpawnAmount; i++)
            {
                _smallAsteroidsObjectPool.GetModelViewPair(out DestroyableDirectedModel model, out AsteroidView view);
                AsteroidModelViewSettings(model, view, position);
                _smallAsteroids.Add(model, view);
                model.OnDestroy += DeactivateSmallAsteroid;
            }
        }

        private void AsteroidModelViewSettings(DestroyableDirectedModel model, AsteroidView view, Vector2 position)
        {
            model.ChangePosition(position);
            model.ChangeDirection(new Vector2(Random.value, Random.value) - model.Position);
            view.ChangePosition(model.Position);
            _collisionHandler.AddCollision(model);
        }

        private void UpdatePositions(Dictionary<DestroyableDirectedModel, AsteroidView> asteroids, float speed)
        {
            foreach (var pair in asteroids)
            {
                var newPosition = pair.Key.Position + pair.Key.Direction * speed * Time.deltaTime;
                newPosition = CameraData.RepeatInViewport(newPosition);

                pair.Key.ChangePosition(newPosition);
                asteroids[pair.Key].ChangePosition(newPosition);
            }
        }

        private void DeactivateBigAsteroid(DestroyableDirectedModel model)
        {
            SpawnSmallAsteroids(model.Position);
            _bigAsteroidsObjectPool.DeactivateModelViewPair(model, _bigAsteroids[model]);
            _bigAsteroids.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateBigAsteroid;
        }

        private void DeactivateSmallAsteroid(DestroyableDirectedModel model)
        {
            _smallAsteroidsObjectPool.DeactivateModelViewPair(model, _smallAsteroids[model]);
            _smallAsteroids.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateSmallAsteroid;
        }
    }
}
