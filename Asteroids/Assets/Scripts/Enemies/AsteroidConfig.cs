using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NewAsteroidConfig", menuName = "Asteroid Config", order = 12)]
    public class AsteroidConfig : ScriptableObject
    {
        public AsteroidView AsteroidViewPrefab;
        public float AsteroidSpeed = 1;
        public float AsteroidFirstSpawnTime = 5;
        public float AsteroidDelaySpawnTime = 2;
    }
}
