using UnityEngine;

using UnityEngine;

// Definición de primitivas geométricas
public struct CustomCircle
{
    public Vector2 center;
    public float radius;

    public CustomCircle(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }
}

public struct CustomAABB
{
    public Vector2 min;
    public Vector2 max;

    public CustomAABB(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }
}

// Algoritmos de detección pura
public static class CustomCollision2D
{
    public static bool CheckCircleAABB(CustomCircle circle, CustomAABB box)
    {
        float closestX = Mathf.Clamp(circle.center.x, box.min.x, box.max.x);
        float closestY = Mathf.Clamp(circle.center.y, box.min.y, box.max.y);

        float distanceX = circle.center.x - closestX;
        float distanceY = circle.center.y - closestY;

        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

        return distanceSquared <= (circle.radius * circle.radius);
    }
}
