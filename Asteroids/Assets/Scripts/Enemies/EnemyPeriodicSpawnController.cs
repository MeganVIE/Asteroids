using UnityEngine;

namespace Enemies
{
    public class EnemyPeriodicSpawnController : EnemySpawnController
    {
        private float _currentSpawnTime;
        private float _timer;
        public EnemyPeriodicSpawnController(EnemyPeriodicConfig config, CollisionHandler collisionHandler, CameraData cameraData) :
            base(config, collisionHandler, cameraData)
        {
            _currentSpawnTime = config.FirstSpawnTime;
            _timer = 0;
        }

        public override void Update()
        {
            base.Update();
            Timer();
        }

        protected void Timer()
        {
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnTime)
            {
                _timer = 0;
                _currentSpawnTime = ((EnemyPeriodicConfig)_enemyConfig).DelaySpawnTime;
                SpawnEnemy(_cameraData.GetRandomPositionOnBound());
            }
        }
    }
}
