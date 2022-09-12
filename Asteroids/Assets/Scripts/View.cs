using UnityEngine;

public class View : MonoBehaviour
{
    protected Transform _transform;
    private Camera _camera;

    private Vector3 _cameraOffset = Vector3.forward;

    private void Awake()
    {
        _transform = transform;
        _camera = Camera.main;
    }

    public void ChangePosition(Vector3 newPosition)
    {
        _transform.position = _camera.ViewportToWorldPoint(newPosition + _cameraOffset);
    }
}