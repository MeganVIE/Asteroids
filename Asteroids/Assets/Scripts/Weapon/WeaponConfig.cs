using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Weapon Config", order = 11)]
    public class WeaponConfig : ScriptableObject
    {
        public float BulletSpeed = 3;
        public BulletView BulletViewPrefab;
        public float BulletCollisionRadius = .05f;

        public LaserView LaserViewPrefab;
        public float LaserCollisionRadius = 10f;
        public float LaserLifeTime = 1f;
        public byte LaserMaxAmount = 5;
        public float LaserAmountRechargeTime = 2f;
    }
}
