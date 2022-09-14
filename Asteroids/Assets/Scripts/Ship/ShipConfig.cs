using UnityEngine;

namespace Ship
{
    [CreateAssetMenu(fileName = "NewShipConfig", menuName = "Ship Config", order = 10)]
    public class ShipConfig : ScriptableObject
    {
        public float CollisionRadius = .1f;
        public float MaxSpeed = 0.001f;
        public float AccelerationSpeed = 0.0005f;
        public float SlowdownSpeed = 1f;
        public float RotationSpeed = 90;
        public Vector2 StartPosition = new Vector2(.5f, .5f);
    }
}
