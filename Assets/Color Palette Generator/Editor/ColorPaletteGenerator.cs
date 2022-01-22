using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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

public class ColorPaletteGenerator : EditorWindow
{
    static Texture2D texture;
    static Shader shader;
    [Range(0.01f, 2f)]
    static float threshold = 0.25f;

    [MenuItem("Tools/Color Palette Generator")]
    static void ShowWindow()
    {
        shader = Shader.Find("Unlit/Color");
        EditorWindow window = GetWindow(typeof(ColorPaletteGenerator));
        window.titleContent = new GUIContent("Color Palette Generator");
        window.maxSize = new Vector2(400f, 250f);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnGUI()
    {
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture 2D", texture, typeof(Texture2D), false);
        shader = (Shader)EditorGUILayout.ObjectField("Shader", shader, typeof(Shader), false);
        threshold = EditorGUILayout.Slider(threshold, 0.1f, 4);
        if (!texture)
        {
            EditorGUILayout.HelpBox("Please select a texture. You can't just extract colors from void.", MessageType.Error);
        }

        if (texture && !texture.isReadable)
        {
            EditorGUILayout.HelpBox("Make sure that selected texture is read/write enabled. Please check import settings", MessageType.Error);
            Selection.activeObject = texture;
        }
        if (GUILayout.Button("Generate") && texture && texture.isReadable)
        {
            string path = EditorUtility.OpenFolderPanel("Save to", "Assets/", "Materials");
            if (path.Length >= Application.dataPath.Length)
            {
                string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
                List<Color> colors = ExtractColorsFromTexture(texture, threshold);
                for (int i = 0; i < colors.Count; i++)
                {
                    Material cM = new Material(shader);
                    cM.color = colors[i];
                    string uniquePath = AssetDatabase.GenerateUniqueAssetPath(relativePath + $"/Color {ColorUtility.ToHtmlStringRGBA(colors[i])}.mat");
                    AssetDatabase.CreateAsset(cM, uniquePath);
                }
                AssetDatabase.SaveAssets();
            }
        }
    }
    private List<Color> ExtractColorsFromTexture(Texture2D t, float threshold)
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < t.width; i++)
        {
            for (int j = 0; j < t.height; j++)
            {
                Color c = t.GetPixel(i, j);
                bool canAdd = colors.TrueForAll((b) => !c.Approximately(b, threshold));
                if (canAdd) colors.Add(c);
            }
        }
        return colors;
    }
}