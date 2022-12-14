using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawnController: IUpdatable, IClearable, IRestartable
    {
        protected CameraData _cameraData;
        protected ObjectPointsConfig _enemyConfig;
        private CollisionHandler _collisionHandler;
        private DirectedModelTransformHandler _transformHandler;

        private DestroyableObjectPool<Enemy, View> _enemiesObjectPool;
        private Dictionary<Enemy, View> _enemies;

        public System.Action<Enemy> OnEnemyDestroyed;
        public EnemySpawnController(ObjectPointsConfig config, CollisionHandler collisionHandler, CameraData cameraData)
        {
            _cameraData = cameraData;
            _collisionHandler = collisionHandler;
            _enemyConfig = config;
            _transformHandler = new DirectedModelTransformHandler();
            _enemiesObjectPool = new DestroyableObjectPool<Enemy, View>(_enemyConfig.ViewPrefab, ObjectType.Enemy, _enemyConfig.CollisionRadius);
            _enemies = new Dictionary<Enemy, View>();
        }

        public virtual void Update()
        {
            UpdatePositions();
        }
        public void Clear()
        {
            _enemies.Clear();
            _enemiesObjectPool.Clear();
        }
        public virtual void Restart()
        {
            foreach (var pair in _enemies)
            {
                _enemiesObjectPool.DeactivateModelViewPair(pair.Key, pair.Value);
                _collisionHandler.RemoveCollision(pair.Key);
            }
            _enemies.Clear();
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

        protected virtual void UpdateModelViewData(Enemy model)
        {
            var newPosition = _transformHandler.GetNewPosition(model, _enemyConfig.Speed);
            newPosition = _cameraData.RepeatInViewport(newPosition);

            model.ChangePosition(newPosition);
            _enemies[model].ChangePosition(newPosition);
        }

        public void SpawnEnemy(Vector2 position)
        {
            _enemiesObjectPool.GetModelViewPair(out Enemy model, out View view);
            EnemyModelViewSettings(model, view, position);
            _enemies.Add(model, view);
            model.OnDestroy += DeactivateEnemy;
        }

        private void EnemyModelViewSettings(Enemy model, View view, Vector2 position)
        {
            model.SetPoints(_enemyConfig.Points);
            model.ChangePosition(position);
            model.ChangeDirection(new Vector2(Random.value, Random.value) - model.Position);
            view.ChangePosition(model.Position);
            _collisionHandler.AddCollision(model);
        }

        private void DeactivateEnemy(DestroyableDirectedModel model)
        {
            var enemyModel = (Enemy)model;
            if (!_enemies.ContainsKey(enemyModel))
                return;

            _enemiesObjectPool.DeactivateModelViewPair(enemyModel, _enemies[enemyModel]);
            _enemies.Remove(enemyModel);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateEnemy;
            OnEnemyDestroyed?.Invoke(enemyModel);
        }
    }
}
