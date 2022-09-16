namespace Enemies
{
    public class AsteroidHandler : IUpdatable
    {
        private EnemySpawnController _smallAsteroidsController;
        private EnemyPeriodicSpawnController _bigAsteroidController;
        private SmallAsteroidConfig _smallConfig;

        public AsteroidHandler(BigAsteroidConfig bigConfig, SmallAsteroidConfig smallConfig, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _smallConfig = smallConfig;
            _bigAsteroidController = new EnemyPeriodicSpawnController(bigConfig, collisionHandler, cameraData);
            _smallAsteroidsController = new EnemySpawnController(_smallConfig, collisionHandler, cameraData);

            _bigAsteroidController.OnEnemyDestroyed += OnBigAsteroidDestroyed;
        }

        void IUpdatable.Update()
        {
            _bigAsteroidController.Update();
            _smallAsteroidsController.Update();
        }

        private void OnBigAsteroidDestroyed(DestroyableDirectedModel model)
        {
            for (int i = 0; i < _smallConfig.SpawnAmount; i++)
            {
                _smallAsteroidsController.SpawnEnemy(model.Position);
            }
        }
    }
}
