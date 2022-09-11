using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class BulletObjectPool
    {
        private BulletView _bulletViewPrefab;
        private Dictionary<BulletModel, BulletView> _unusedBullets;

        public BulletObjectPool(BulletView bulletViewPrefab)
        {
            _bulletViewPrefab = bulletViewPrefab;
            _unusedBullets = new Dictionary<BulletModel, BulletView>();
        }

        public void GetBullet(out BulletModel model, out BulletView view)
        {
            if (_unusedBullets.Count == 0)
            {
                model = new BulletModel();
                view = Object.Instantiate(_bulletViewPrefab);
            }
            else
            {
                model = _unusedBullets.Keys.First();
                view = _unusedBullets[model];
                view.gameObject.SetActive(true);
                _unusedBullets.Remove(model);
            }
        }

        public void DeactivateBullet(BulletModel model, BulletView view)
        {
            _unusedBullets.Add(model, view);
            view.gameObject.SetActive(false);
        }
    }
}