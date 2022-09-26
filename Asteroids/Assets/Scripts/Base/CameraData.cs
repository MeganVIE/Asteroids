using UnityEngine;

public class CameraData
{
    private float _halfViewportHeight;
    private float _halfViewportWidth;

    public CameraData(Camera camera)
    {
        _halfViewportHeight = Camera.main.orthographicSize;
        _halfViewportWidth = _halfViewportHeight * Camera.main.aspect;
    }

    public bool IsOutsideViewport(Vector2 position)
    {
        return (Mathf.Abs(position.y) > _halfViewportHeight) || (Mathf.Abs(position.x) > _halfViewportWidth);
    }

    public Vector2 RepeatInViewport(Vector2 position)
    {
        RepeatInViewportVectorComponent(ref position.x, _halfViewportWidth);
        RepeatInViewportVectorComponent(ref position.y, _halfViewportHeight);
        return position;
    }

    public Vector2 GetRandomPositionOnBound()
    {
        Vector2 position;
        if (GetRandomFiftyFifty())
            SetVectorComponentsValue(out position.x, out position.y, _halfViewportWidth, _halfViewportHeight);
        else
            SetVectorComponentsValue(out position.y, out position.x, _halfViewportHeight, _halfViewportWidth);
        return position;
    }

    private bool GetRandomFiftyFifty() => Random.Range(0, 2) == 0;
    private void SetVectorComponentsValue(out float componentA, out float componentB, float limitA, float limitB)
    {
        componentA = GetRandomFiftyFifty() ? limitA : -limitA;
        componentB = Random.Range(-limitB, limitB);
    }

    private void RepeatInViewportVectorComponent(ref float component, float limit)
    {
        component = component > limit ? -limit : component < -limit ? limit : component;
    }
}