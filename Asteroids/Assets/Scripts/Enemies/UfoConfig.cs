using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NewUfoConfig", menuName = "Ufo Config", order = 13)]
    public class UfoConfig : ScriptableObject
    {
        public float CollisionRadius = .23f;
        public View ViewPrefab;
        public float Speed = .4f;
        public float FirstSpawnTime = 10;
        public float DelaySpawnTime = 5;
    }
}
