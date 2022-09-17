using UnityEngine.Events;

namespace Enemies
{
    public class AsteroidHandler : IUpdatable
    {
        private EnemySpawnController _smallAsteroidsController;
        private EnemyPeriodicSpawnController _bigAsteroidController;
        private SmallAsteroidConfig _smallConfig;

        public UnityEvent<Enemy> OnEnemyDestroyed;
        public AsteroidHandler(BigAsteroidConfig bigConfig, SmallAsteroidConfig smallConfig, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _smallConfig = smallConfig;
            _bigAsteroidController = new EnemyPeriodicSpawnController(bigConfig, collisionHandler, cameraData);
            _smallAsteroidsController = new EnemySpawnController(_smallConfig, collisionHandler, cameraData);

            _bigAsteroidController.OnEnemyDestroyed.AddListener(OnBigAsteroidDestroyed);
            _smallAsteroidsController.OnEnemyDestroyed.AddListener(OnAsteroidDestroyed);
            OnEnemyDestroyed = new UnityEvent<Enemy>();
        }

        void IUpdatable.Update()
        {
            _bigAsteroidController.Update();
            _smallAsteroidsController.Update();
        }
        void IUpdatable.Clear()
        {
            OnEnemyDestroyed.RemoveAllListeners();
            _bigAsteroidController.Clear();
            _smallAsteroidsController.Clear();
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
