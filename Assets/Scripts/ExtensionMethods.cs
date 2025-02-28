using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Scales a vector to lie on the circumference of an ellipse.
    /// </summary>
    /// <param name="vector">The input vector.</param>
    /// <param name="ellipseWidth">The width of the ellipse (horizontal diameter).</param>
    /// <param name="ellipseHeight">The height of the ellipse (vertical diameter).</param>
    /// <returns>A new vector scaled to the circumference of the ellipse.</returns>
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

    /// <summary>
    /// Draws an ellipse using the built-in debug tools.
    /// </summary>
    /// <param name="center">The center of the ellipse.</param>
    /// <param name="ellipseWidth">The width of the ellipse (horizontal diameter).</param>
    /// <param name="ellipseHeight">The height of the ellipse (vertical diameter).</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="segments">The number of lines used to draw the ellipse.</param>
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

    /// <summary>
    /// Picks a random element from an array.
    /// </summary>
    /// <param name="array">The input array.</param>
    public static T SelectRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
