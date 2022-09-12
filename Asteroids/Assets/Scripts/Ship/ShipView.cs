using UnityEngine;

namespace Ship
{
    public class ShipView : View
    {
        public void ChangeRotation(float newRotation)
        {
            _transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }
}