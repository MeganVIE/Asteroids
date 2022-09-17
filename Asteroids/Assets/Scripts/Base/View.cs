using UnityEngine;

public class View : MonoBehaviour
{
    protected Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void ChangePosition(Vector3 newPosition)
    {
        _transform.position = newPosition;
    }
}