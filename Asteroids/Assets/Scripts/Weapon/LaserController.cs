using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Ship;
using System;

namespace Weapon
{
    public class LaserController : BaseBulletHandler<LaserModel, LaserView>, IUpdatable
    {
        private Dictionary<LaserModel, float> _laserTimers;

        private byte _lasersCurrentAmount;
        private float _rechargeTimer;

        public bool CanCreateLaser => _lasersCurrentAmount > 0;
        public byte LasersAmount => _lasersCurrentAmount;

        public Action<byte> OnCurrentAmountChange { get; set; }
        public Action<float> OnRechargeTimeChange { get; set; }

        public LaserController(WeaponConfig weaponConfig, ShipModel model, CollisionHandler collisionHandler) :
            base(weaponConfig, model, collisionHandler)
        {
            _bulletObjectPool = new ObjectPool<LaserModel, LaserView>(_weaponConfig.LaserViewPrefab, ObjectType.Laser, _weaponConfig.LaserCollisionRadius);
            _laserTimers = new Dictionary<LaserModel, float>();

            Restart();
        }

        public override void Update()
        {
            TimersUpdate();
            LaserRecharge();
        }
        public override void Clear()
        {
            base.Clear();
            _laserTimers.Clear();
        }
        public override void Restart()
        {
            base.Restart();
            _rechargeTimer = 0;
            _lasersCurrentAmount = _weaponConfig.LaserMaxAmount;
            _laserTimers.Clear();
        }

        protected override void ModelViewSettings(LaserModel model, LaserView view)
        {
            base.ModelViewSettings(model, view);

            view.ChangeRotation(_model.Rotation);
            view.SetLineView(model.CollisionRadius, model.Direction);
            _laserTimers.Add(model, 0);
            AmountChange(false);
        }

        private void TimersUpdate()
        {
            if (_bullets.Count == 0)
                return;

            foreach (var model in _laserTimers.Keys.ToArray())
            {
                _laserTimers[model] += Time.deltaTime;
                if (_laserTimers[model] >= _weaponConfig.LaserLifeTime)
                    DeactivateLaser(model);
            }
        }

        private void LaserRecharge()
        {
            if (_lasersCurrentAmount >= _weaponConfig.LaserMaxAmount)
                return;

            _rechargeTimer += Time.deltaTime;
            if (_rechargeTimer >= _weaponConfig.LaserAmountRechargeTime)
            {
                _rechargeTimer = 0;
                AmountChange(true);
            }
            OnRechargeTimeChange?.Invoke(_rechargeTimer == 0 ? 0 : _weaponConfig.LaserAmountRechargeTime - _rechargeTimer);
        }

        private void AmountChange(bool increase)
        {
            _lasersCurrentAmount += (byte)(increase ? 1 : -1);
            OnCurrentAmountChange?.Invoke(_lasersCurrentAmount);
        }

        private void DeactivateLaser(LaserModel model)
        {
            base.DeactivateBullet(model);
            _laserTimers.Remove(model);
        }
    }
}
