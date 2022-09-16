using System.Linq;
using UnityEngine;
using Ship;

namespace Weapon
{
    public class BulletController : BaseBulletHandler<DestroyableDirectedModel, BulletView>, IUpdatable
    {
        public BulletController(WeaponConfig weaponConfig, ShipModel model, CollisionHandler collisionHandler) :
            base(weaponConfig, model, collisionHandler)
        {
            _bulletObjectPool = new ObjectPool<DestroyableDirectedModel, BulletView>(_weaponConfig.BulletViewPrefab, ObjectType.Bullet,_weaponConfig.BulletCollisionRadius);
        }

        void IUpdatable.Update()
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

        protected override void ModelViewSettings(DestroyableDirectedModel model, BulletView view)
        {
            base.ModelViewSettings(model, view);
            model.OnDestroy += DeactivateBullet;
        }

        protected override void DeactivateBullet(DestroyableDirectedModel model)
        {
            base.DeactivateBullet(model);
            model.OnDestroy -= DeactivateBullet;
        }

        private void UpdateBulletPosition(DestroyableDirectedModel model, out Vector2 newPosition)
        {
            newPosition = model.Position + model.Direction * _weaponConfig.BulletSpeed * Time.deltaTime;
            model.ChangePosition(newPosition);
            _bullets[model].ChangePosition(newPosition);
        }
    }
}
