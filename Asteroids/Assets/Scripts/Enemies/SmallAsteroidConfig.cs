using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "NewSmallAsteroidConfig", menuName = "Small Asteroid Config", order = 12)]
    public class SmallAsteroidConfig : ObjectPointsConfig
    {
        public byte SpawnAmount = 2;
    }
}
