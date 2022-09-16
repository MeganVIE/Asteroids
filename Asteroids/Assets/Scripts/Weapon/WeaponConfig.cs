using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Weapon Config", order = 11)]
    public class WeaponConfig : SpawnObjectConfig
    {
        [Space]
        public LaserView LaserViewPrefab;
        public float LaserCollisionRadius = 10f;
        public float LaserLifeTime = 1f;
        public byte LaserMaxAmount = 5;
        public float LaserAmountRechargeTime = 2f;
    }
}
