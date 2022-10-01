using UnityEngine;

namespace Enemies
{
    public class EnemyPeriodicSpawnController : EnemySpawnController
    {
        private float _firstSpawnTime;
        private float _currentSpawnTime;
        private float _timer;
        public EnemyPeriodicSpawnController(EnemyPeriodicConfig config, CollisionHandler collisionHandler, CameraData cameraData) :
            base(config, collisionHandler, cameraData)
        {
            _firstSpawnTime = config.FirstSpawnTime;
            _currentSpawnTime = _firstSpawnTime;
            _timer = 0;
        }

        public override void Update()
        {
            base.Update();
            Timer();
        }
        public override void Restart()
        {
            base.Restart();
            _currentSpawnTime = _firstSpawnTime;
            _timer = 0;
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
