using System.Collections.Generic;
using UnityEngine;

public class ColorPaletteGenerator
{
    public static List<Color> ExtractColorsFromTexture(Texture2D texture, float threshold)
    {
        if (!texture.isReadable)
        {
            Debug.LogError("Make sure that texture is read/write enabled. Please check import settings.");
            return null;
        }

        List<Color> colors = new List<Color>();
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                Color c = texture.GetPixel(i, j);
                bool canAdd = colors.TrueForAll((b) => !c.Approximately(b, threshold));
                if (canAdd) colors.Add(c);
            }
        }
        return colors;
    }

    public static Material CreateMaterial(Shader shader, Color color)
    {
        Material material = new Material(shader);
        material.color = color;
        return material;
    }

    public static List<Material> CreateMaterials(Shader shader, List<Color> colors)
    {
        List<Material> materials = new List<Material>();
        foreach (Color color in colors)
        {
            materials.Add(CreateMaterial(shader, color));
        }
        return materials;
    }
}
