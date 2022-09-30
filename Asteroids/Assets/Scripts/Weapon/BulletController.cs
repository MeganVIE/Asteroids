using System.Linq;
using UnityEngine;
using Ship;

namespace Weapon
{
    public class BulletController : BaseBulletHandler<DestroyableDirectedModel, View>
    {
        private CameraData _cameraData;
        private DirectedModelTransformHandler _transformHandler;
        public BulletController(WeaponConfig weaponConfig, ShipModel model, CollisionHandler collisionHandler, CameraData cameraData) :
            base(weaponConfig, model, collisionHandler)
        {
            _cameraData = cameraData;
            _transformHandler = new DirectedModelTransformHandler();
            _bulletObjectPool = new DestroyableObjectPool<DestroyableDirectedModel, View>(_weaponConfig.ViewPrefab, ObjectType.Bullet,_weaponConfig.CollisionRadius);
        }

        public override void Update()
        {
            if (_bullets.Count == 0)
                return;

            var bulletModels = _bullets.Keys.ToArray();
            for (int i = 0; i < bulletModels.Length; i++)
            {
                UpdateBulletPosition(bulletModels[i], out Vector2 newPosition);
                
                if (_cameraData.IsOutsideViewport(newPosition))
                    DeactivateBullet(bulletModels[i]);
            }
        }

        protected override void ModelViewSettings(DestroyableDirectedModel model, View view)
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
            newPosition = _transformHandler.GetNewPosition(model, _weaponConfig.Speed);
            model.ChangePosition(newPosition);
            _bullets[model].ChangePosition(newPosition);
        }
    }
}
