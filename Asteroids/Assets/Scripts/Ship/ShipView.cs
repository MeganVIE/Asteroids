using UnityEngine;

namespace Ship
{
    public class ShipView : MonoBehaviour
    {
        private ModelTransform _modelTransform;

        private Transform _transform;
        private Camera _camera;

        private Vector3 _cameraOffset = Vector3.forward;

        private void Start()
        {
            _transform = transform;
            _camera = Camera.main;
        }

        public void SetModelTransform(ModelTransform modelTransform)
        {
            _modelTransform = modelTransform;
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            _transform.position = _camera.ViewportToWorldPoint((Vector3)_modelTransform.Position + _cameraOffset);
        }
        public void UpdateRotation()
        {
            _transform.rotation = Quaternion.Euler(0, 0, _modelTransform.Rotation);
        }
    }
}