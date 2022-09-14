using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NewAsteroidConfig", menuName = "Asteroid Config", order = 12)]
    public class AsteroidConfig : ScriptableObject
    {
        public float CollisionRadius = .5f;
        public AsteroidView AsteroidViewPrefab;
        public float AsteroidSpeed = .1f;
        public float AsteroidFirstSpawnTime = 3;
        public float AsteroidDelaySpawnTime = 2;
    }
}
