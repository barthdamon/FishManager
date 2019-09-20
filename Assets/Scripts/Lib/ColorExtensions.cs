using UnityEngine;

public static class ColorExtensions
{
    /// <summary>
    /// Create a new Color based on an existing Color.
    /// </summary>
    /// <param name="colour">An existing colour.</param>
    /// <param name="a">The adjusted alpha value, or -1f to leave unchanged.</param>
    /// <param name="r">The adjusted red value, or -1f to leave unchanged.</param>
    /// <param name="g">The adjusted green value, or -1f to leave unchanged.</param>
    /// <param name="b">The adjusted blue value, or -1f to leave unchanged.</param>
    /// <returns></returns>
    public static Color Adjusted(this Color colour, float a = -1f, float r = -1f, float g = -1f, float b = -1f)
    {
        Color new_colour = colour;
        new_colour.a = a >= 0f ? a : new_colour.a;
        new_colour.r = r >= 0f ? r : new_colour.r;
        new_colour.g = g >= 0f ? g : new_colour.g;
        new_colour.b = b >= 0f ? b : new_colour.b;
        return new_colour;
    }
}