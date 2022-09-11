using UnityEngine;

namespace Ship
{
    public class ShipModel
    {
        public ModelTransform modelTransform { get; private set; }

        public ShipModel(Vector2 startPosition)
        {
            modelTransform = new ModelTransform(startPosition);
        }
    }
}