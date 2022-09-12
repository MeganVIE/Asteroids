using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class BulletController : IController
    {
        private ModelTransform _shipModelTransform;
        private WeaponConfig _weaponConfig;
        private ObjectPool<BulletModel, BulletView> _bulletObjectPool;
        private Dictionary<BulletModel, BulletView> _bullets;

        public BulletController(WeaponConfig weaponConfig, ModelTransform shipModelTransform)
        {
            _shipModelTransform = shipModelTransform;
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

                if (IsOutsideViewport(newPosition))
                    DeactivateBullet(bulletModels[i]);
            }
        }

        public void CreateBullet()
        {
            var direction = Quaternion.Euler(0, 0, _shipModelTransform.Rotation) * Vector3.up;

            _bulletObjectPool.GetObject(out BulletModel model, out BulletView view);
            model.ChangePosition(_shipModelTransform.Position);
            model.ChangeDirection(direction);
            view.ChangePosition(model.Position);
            _bullets.Add(model, view);
        }

        private void UpdateBulletPosition(BulletModel model, out Vector2 newPosition)
        {
            newPosition = model.Position + model.Direction * _weaponConfig.BulletSpeed * Time.deltaTime;
            model.ChangePosition(newPosition);
            _bullets[model].ChangePosition(newPosition);
        }

        private bool IsOutsideViewport(Vector2 position)
        {
            return (position.x < 0 || position.x > 1) || (position.y < 0 || position.y > 1);
        }

        private void DeactivateBullet(BulletModel model)
        {
            _bulletObjectPool.DeactivateObject(model, _bullets[model]);
            _bullets.Remove(model);
        }
    }
}
