using Ship;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class UfoController : IUpdatable
    {
        private ShipModel _shipModel;
        private UfoConfig _ufoConfig;
        private CollisionHandler _collisionHandler;
        private ObjectPool<DestroyableDirectedModel, View> _ufosObjectPool;
        private Dictionary<DestroyableDirectedModel, View> _ufos;

        private float _currentSpawnTime;
        private float _timer;
        public UfoController(UfoConfig ufoConfig, CollisionHandler collisionHandler, ShipModel shipModel)
        {
            _shipModel = shipModel;
            _collisionHandler = collisionHandler;
            _ufoConfig = ufoConfig;
            _ufosObjectPool = new ObjectPool<DestroyableDirectedModel, View>(_ufoConfig.ViewPrefab, ObjectType.Enemy, _ufoConfig.CollisionRadius);
            _ufos = new Dictionary<DestroyableDirectedModel, View>();

            _currentSpawnTime = _ufoConfig.FirstSpawnTime;
            _timer = 0;
        }

        void IUpdatable.Update()
        {
            Timer();

            if (_ufos.Count > 0)
                UpdatePositions();
        }

        private void Timer()
        {
            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnTime)
            {
                _timer = 0;
                _currentSpawnTime = _ufoConfig.DelaySpawnTime;
                SpawnUfo();
            }
        }

        private void SpawnUfo()
        {
            _ufosObjectPool.GetModelViewPair(out DestroyableDirectedModel model, out View view);
            model.ChangePosition(CameraData.GetRandomPositionOnBound());
            model.ChangeDirection(new Vector2(Random.value, Random.value) - model.Position);
            view.ChangePosition(model.Position);
            _collisionHandler.AddCollision(model);
            _ufos.Add(model, view);
            model.OnDestroy += DeactivateUfo;
        }

        private void UpdatePositions()
        {
            foreach (var pair in _ufos)
            {
                var newDirection = (_shipModel.Position - pair.Key.Position).normalized;
                pair.Key.ChangeDirection(newDirection);

                var newPosition = pair.Key.Position + pair.Key.Direction * _ufoConfig.Speed * Time.deltaTime;
                pair.Key.ChangePosition(newPosition);
                _ufos[pair.Key].ChangePosition(newPosition);
            }
        }

        private void DeactivateUfo(DestroyableDirectedModel model)
        {
            _ufosObjectPool.DeactivateModelViewPair(model, _ufos[model]);
            _ufos.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateUfo;
        }
    }
}
