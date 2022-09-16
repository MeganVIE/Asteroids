using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Ship;

namespace Weapon
{
    public class LaserController : BaseBulletHandler<LaserModel, LaserView>, IUpdatable
    {
        private Dictionary<LaserModel, float> _laserTimers;

        private byte _lasersCurrentAmount;
        private float _rechargeTimer;

        public bool CanCreateLaser => _lasersCurrentAmount > 0;
        public byte CurrentAmount => _lasersCurrentAmount;
        public float RechargeTime => _lasersCurrentAmount >= _weaponConfig.LaserMaxAmount ? 0 : _weaponConfig.LaserAmountRechargeTime - _rechargeTimer;

        public LaserController(WeaponConfig weaponConfig, ShipModel model, CollisionHandler collisionHandler) :
            base(weaponConfig, model, collisionHandler)
        {
            _bulletObjectPool = new ObjectPool<LaserModel, LaserView>(_weaponConfig.LaserViewPrefab, ObjectType.Laser, _weaponConfig.LaserCollisionRadius);
            _laserTimers = new Dictionary<LaserModel, float>();

            _rechargeTimer = 0;
            _lasersCurrentAmount = _weaponConfig.LaserMaxAmount;
        }

        void IUpdatable.Update()
        {
            if (_bullets.Count > 0)
            {
                foreach (var model in _laserTimers.Keys.ToArray())
                {
                    _laserTimers[model] += Time.deltaTime;
                    if (_laserTimers[model] >= _weaponConfig.LaserLifeTime)
                        DeactivateLaser(model);
                }
            }

            if (_lasersCurrentAmount < _weaponConfig.LaserMaxAmount)
            {
                _rechargeTimer += Time.deltaTime;
                if (_rechargeTimer >= _weaponConfig.LaserAmountRechargeTime)
                {
                    _rechargeTimer = 0;
                    _lasersCurrentAmount++;
                }
            }
        }
        protected override void ModelViewSettings(LaserModel model, LaserView view)
        {
            base.ModelViewSettings(model, view);

            view.ChangeRotation(_model.Rotation);
            view.SetLineView(model.CollisionRadius, model.Direction);
            _laserTimers.Add(model, 0);
            _lasersCurrentAmount--;
        }
        private void DeactivateLaser(LaserModel model)
        {
            base.DeactivateBullet(model);
            _laserTimers.Remove(model);
        }
    }
}
