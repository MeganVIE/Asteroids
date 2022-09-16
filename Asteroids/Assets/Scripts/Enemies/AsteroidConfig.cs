using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NewAsteroidConfig", menuName = "Asteroid Config", order = 12)]
    public class AsteroidConfig : ScriptableObject
    {
        public float BigAsteroidCollisionRadius = .5f;
        public AsteroidView BigAsteroidViewPrefab;
        public float BigAsteroidSpeed = .1f;
        public float AsteroidFirstSpawnTime = 3;
        public float AsteroidDelaySpawnTime = 2;

        public byte SmallAsteroidSpawnAmount = 2;
        public AsteroidView SmallAsteroidViewPrefab;
        public float SmallAsteroidCollisionRadius = .3f;
        public float SmallAsteroidSpeed = .5f;
    }
}
