using UnityEngine;

public class ShipView : MonoBehaviour
{
    private ShipModel _model;

    private Transform _transform;
    private Camera _camera;

    private Vector3 _cameraOffset = Vector3.forward;

    private void Start()
    {
        _transform = transform;
        _camera = Camera.main;
    }

    public void SetModel(ShipModel model)
    {
        _model = model;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        _transform.position = _camera.ViewportToWorldPoint((Vector3)_model.Position + _cameraOffset);
    }
    public void UpdateRotation()
    {
        _transform.rotation = Quaternion.Euler(0, 0, _model.Rotation);
    }
}
