using UnityEngine;

namespace Weapon
{
    public class LaserView : View
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public void SetLineView(float radius, Vector2 direction)
        {
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _transform.position);

            var endPosition = direction.normalized * radius + (Vector2)_transform.position;
            _lineRenderer.SetPosition(1, endPosition);
        }

        public void ChangeRotation(float newRotation)
        {
            _transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }
}
