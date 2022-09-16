using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawnController: IUpdatable
    {
        protected CameraData _cameraData;
        protected SpawnObjectConfig _enemyConfig;
        private CollisionHandler _collisionHandler;

        private ObjectPool<DestroyableDirectedModel, View> _enemiesObjectPool;
        private Dictionary<DestroyableDirectedModel, View> _enemies;

        public System.Action<DestroyableDirectedModel> OnEnemyDestroyed;
        public EnemySpawnController(SpawnObjectConfig config, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _cameraData = cameraData;
            _collisionHandler = collisionHandler;
            _enemyConfig = config;
            _enemiesObjectPool = new ObjectPool<DestroyableDirectedModel, View>(_enemyConfig.ViewPrefab, ObjectType.Enemy, _enemyConfig.CollisionRadius);
            _enemies = new Dictionary<DestroyableDirectedModel, View>();
        }

        public virtual void Update()
        {
            UpdatePositions();
        }

        protected void UpdatePositions()
        {
            if (_enemies.Count == 0)
                return;

            foreach (var pair in _enemies)
            {
                UpdateModelViewData(pair.Key);
            }
        }

        protected virtual void UpdateModelViewData(DestroyableDirectedModel model)
        {
            var newPosition = model.Position + model.Direction * _enemyConfig.Speed * Time.deltaTime;
            newPosition = _cameraData.RepeatInViewport(newPosition);

            model.ChangePosition(newPosition);
            _enemies[model].ChangePosition(newPosition);
        }

        public void SpawnEnemy(Vector2 position)
        {
            _enemiesObjectPool.GetModelViewPair(out DestroyableDirectedModel model, out View view);
            EnemyModelViewSettings(model, view, position);
            _enemies.Add(model, view);
            model.OnDestroy += DeactivateEnemy;
        }

        private void EnemyModelViewSettings(DestroyableDirectedModel model, View view, Vector2 position)
        {
            model.ChangePosition(position);
            model.ChangeDirection(new Vector2(Random.value, Random.value) - model.Position);
            view.ChangePosition(model.Position);
            _collisionHandler.AddCollision(model);
        }

        private void DeactivateEnemy(DestroyableDirectedModel model)
        {
            _enemiesObjectPool.DeactivateModelViewPair(model, _enemies[model]);
            _enemies.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateEnemy;
            OnEnemyDestroyed?.Invoke(model);
        }
    }
}
