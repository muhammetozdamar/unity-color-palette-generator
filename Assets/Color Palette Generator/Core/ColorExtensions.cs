using UnityEngine;

public static class ColorExtensions
{
    public static bool Approximately(this Color a, Color b, float threshold = 0.25f)
    {
        float distance = Mathf.Sqrt
        (
            Mathf.Pow((a.r - b.r), 2) +
            Mathf.Pow((a.g - b.g), 2) +
            Mathf.Pow((a.b - b.b), 2) +
            Mathf.Pow((a.a - b.a), 2)
        );
        return distance <= threshold;
    }
}