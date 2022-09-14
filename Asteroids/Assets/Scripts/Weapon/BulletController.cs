using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Ship;

namespace Weapon
{
    public class BulletController : IController
    {
        private CollisionHandler _collisionHandler;
        private ShipModel _model;
        private WeaponConfig _weaponConfig;
        private ObjectPool<BulletModel, BulletView> _bulletObjectPool;
        private Dictionary<BulletModel, BulletView> _bullets;

        public BulletController(WeaponConfig weaponConfig, ShipModel model, CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _model = model;
            _weaponConfig = weaponConfig;
            _bulletObjectPool = new ObjectPool<BulletModel, BulletView>(_weaponConfig.BulletViewPrefab);
            _bullets = new Dictionary<BulletModel, BulletView>();
        }

        void IController.Update()
        {
            if (_bullets.Count == 0)
                return;

            var bulletModels = _bullets.Keys.ToArray();
            for (int i = 0; i < bulletModels.Length; i++)
            {
                UpdateBulletPosition(bulletModels[i], out Vector2 newPosition);

                if (CameraData.IsOutsideViewport(newPosition))
                    DeactivateBullet(bulletModels[i]);
            }
        }

        public void CreateBullet()
        {
            var direction = Quaternion.Euler(0, 0, _model.Rotation) * Vector3.up;

            _bulletObjectPool.GetObject(out BulletModel model, out BulletView view);
            model.ChangePosition(_model.Position);
            model.ChangeDirection(direction);
            model.SetCollisionRadius(_weaponConfig.CollisionRadius);
            view.ChangePosition(model.Position);
            _bullets.Add(model, view);
            _collisionHandler.AddCollision(model);

            model.OnDestroy += DeactivateBullet;
        }

        private void UpdateBulletPosition(BulletModel model, out Vector2 newPosition)
        {
            newPosition = model.Position + model.Direction * _weaponConfig.BulletSpeed * Time.deltaTime;
            model.ChangePosition(newPosition);
            _bullets[model].ChangePosition(newPosition);
        }

        private void DeactivateBullet(BulletModel model)
        {
            _bulletObjectPool.DeactivateObject(model, _bullets[model]);
            _bullets.Remove(model);
            _collisionHandler.RemoveCollision(model);
            model.OnDestroy -= DeactivateBullet;
        }
    }
}
