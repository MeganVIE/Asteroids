using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Weapon Config", order = 11)]
    public class WeaponConfig : ScriptableObject
    {
        public float BulletSpeed = 3;
        public BulletView BulletViewPrefab;
        public float CollisionRadius = .05f;
    }
}
