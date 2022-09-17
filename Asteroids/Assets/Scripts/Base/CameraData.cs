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
        position.x = position.x > _halfViewportWidth ? -_halfViewportWidth : position.x < -_halfViewportWidth ? _halfViewportWidth : position.x;
        position.y = position.y > _halfViewportHeight ? -_halfViewportHeight : position.y < -_halfViewportHeight ? _halfViewportHeight : position.y;
        return position;
    }

    public Vector2 GetRandomPositionOnBound()
    {
        Vector2 position;
        if (Random.Range(0, 2) == 0)
        {
            position.x = Random.Range(0, 2) == 0 ? _halfViewportWidth : -_halfViewportWidth;
            position.y = Random.Range(-_halfViewportHeight, _halfViewportHeight);
        }
        else
        {
            position.x = Random.Range(-_halfViewportWidth, _halfViewportWidth);
            position.y = Random.Range(0, 2) == 0 ? _halfViewportHeight : -_halfViewportHeight;
        }
        return position;
    }
}