using UnityEngine;

namespace Ship
{
    [CreateAssetMenu(fileName = "NewShipConfig", menuName = "Ship Config", order = 10)]
    public class ShipConfig : ScriptableObject
    {
        public ShipView ViewPrefab;
        public float CollisionRadius = .3f;
        public float MaxSpeed = 0.015f;
        public float AccelerationSpeed = 0.005f;
        public float SlowdownSpeed = 1f;
        public float RotationSpeed = 90;
        public Vector2 StartPosition = Vector2.zero;
    }
}
