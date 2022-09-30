using System;

namespace Enemies
{
    public class AsteroidHandler : IUpdatable, IClearable
    {
        private EnemySpawnController _smallAsteroidsController;
        private EnemyPeriodicSpawnController _bigAsteroidController;
        private SmallAsteroidConfig _smallConfig;

        public Action<Enemy> OnEnemyDestroyed;
        public AsteroidHandler(BigAsteroidConfig bigConfig, SmallAsteroidConfig smallConfig, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _smallConfig = smallConfig;
            _bigAsteroidController = new EnemyPeriodicSpawnController(bigConfig, collisionHandler, cameraData);
            _smallAsteroidsController = new EnemySpawnController(_smallConfig, collisionHandler, cameraData);

            _bigAsteroidController.OnEnemyDestroyed += OnBigAsteroidDestroyed;
            _smallAsteroidsController.OnEnemyDestroyed += OnAsteroidDestroyed;
        }

        void IUpdatable.Update()
        {
            _bigAsteroidController.Update();
            _smallAsteroidsController.Update();
        }
        void IClearable.Clear()
        {
            _bigAsteroidController.Clear();
            _smallAsteroidsController.Clear();
            _bigAsteroidController.OnEnemyDestroyed -= OnBigAsteroidDestroyed;
            _smallAsteroidsController.OnEnemyDestroyed -= OnAsteroidDestroyed;
        }

        private void OnBigAsteroidDestroyed(Enemy model)
        {
            for (int i = 0; i < _smallConfig.SpawnAmount; i++)
            {
                _smallAsteroidsController.SpawnEnemy(model.Position);
            }
            OnAsteroidDestroyed(model);
        }
        private void OnAsteroidDestroyed(Enemy model) => OnEnemyDestroyed?.Invoke(model);
    }
}
