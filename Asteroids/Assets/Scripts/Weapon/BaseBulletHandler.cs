using System.Collections.Generic;
using UnityEngine;
using Ship;

namespace Weapon
{
    public abstract class BaseBulletHandler<T1, T2> : IUpdatable, IClearable where T1 : DirectedModel, new() where T2: View
    {
        private CollisionHandler _collisionHandler;

        protected ShipModel _model;
        protected WeaponConfig _weaponConfig;
        protected ObjectPool<T1, T2> _bulletObjectPool;
        protected Dictionary<T1, T2> _bullets;

        public BaseBulletHandler(WeaponConfig weaponConfig, ShipModel model, CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _model = model;
            _weaponConfig = weaponConfig;

            _bullets = new Dictionary<T1, T2>();
        }

        public abstract void Update();
        public virtual void Clear()
        {
            _bulletObjectPool.Clear();
            _bullets.Clear();
        }

        public void CreateBullet()
        {
            _bulletObjectPool.GetModelViewPair(out T1 model, out T2 view);
            ModelViewSettings(model, view);
        }

        protected virtual void ModelViewSettings(T1 model, T2 view)
        {
            var direction = Quaternion.Euler(0, 0, _model.Rotation) * Vector3.up;
            model.ChangePosition(_model.Position);
            model.ChangeDirection(direction);
            view.ChangePosition(model.Position);
            _bullets.Add(model, view);
            _collisionHandler.AddCollision(model);
        }

        protected virtual void DeactivateBullet(T1 model)
        {
            _bulletObjectPool.DeactivateModelViewPair(model, _bullets[model]);
            _bullets.Remove(model);
            _collisionHandler.RemoveCollision(model);
        }
    }
}
