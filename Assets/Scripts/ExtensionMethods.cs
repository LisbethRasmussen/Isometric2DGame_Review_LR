using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 ScaleToEllipse(this Vector2 vector, float ellipseWidth, float ellipseHeight)
    {
        if (vector == Vector2.zero)
        {
            return Vector2.zero;
        }

        float a = ellipseWidth;
        float b = ellipseHeight;

        float x = vector.x;
        float y = vector.y;

        float ellipseFactor = (x * x) / (a * a) + (y * y) / (b * b);
        float scale = 1f / Mathf.Sqrt(ellipseFactor);

        return vector * scale;
    }

    public static void DrawEllipse(Vector3 center, float ellipseWidth, float ellipseHeight, Color color, int segments = 50)
    {
        float angleIncrement = Mathf.PI * 2f / segments;
        Vector3 previousPoint = center + new Vector3(ellipseWidth, 0f, 0f);

        for (int i = 0; i < segments; i++)
        {
            float angle = (i + 1) * angleIncrement;
            float dx = Mathf.Cos(angle) * ellipseWidth;
            float dy = Mathf.Sin(angle) * ellipseHeight;
            Vector3 nextPoint = center + new Vector3(dx, dy, 0f);

            Debug.DrawLine(previousPoint, nextPoint, color);
            previousPoint = nextPoint;
        }
    }

    public static T SelectRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
