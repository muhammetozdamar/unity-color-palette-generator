using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ColorPaletteGeneratorEditor : EditorWindow
{
    static Texture2D texture;
    static Shader shader;
    [Range(0.01f, 2f)]
    static float threshold = 0.25f;

    [MenuItem("Tools/Color Palette Generator")]
    static void ShowWindow()
    {
        shader = Shader.Find("Unlit/Color");
        EditorWindow window = GetWindow(typeof(ColorPaletteGeneratorEditor));
        window.titleContent = new GUIContent("Color Palette Generator");
        window.maxSize = new Vector2(400f, 250f);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnGUI()
    {
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture 2D", texture, typeof(Texture2D), false);
        shader = (Shader)EditorGUILayout.ObjectField("Shader", shader, typeof(Shader), false);
        threshold = EditorGUILayout.Slider("Color Threshold", threshold, 0.1f, 4f);

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
            string savePath = EditorUtility.OpenFolderPanel("Save to", "Assets/", "Materials");
            if (savePath.Length >= Application.dataPath.Length)
            {
                string relativePath = "Assets" + savePath.Substring(Application.dataPath.Length);
                List<Color> colors = ColorPaletteGenerator.ExtractColorsFromTexture(texture, threshold);
                List<Material> materials = ColorPaletteGenerator.CreateMaterials(shader, colors);
                for (int i = 0; i < materials.Count; i++)
                {
                    string uniquePath = AssetDatabase.GenerateUniqueAssetPath(relativePath + $"/Color {ColorUtility.ToHtmlStringRGBA(colors[i])}.mat");
                    AssetDatabase.CreateAsset(materials[i], uniquePath);
                }
                AssetDatabase.SaveAssets();
            }
        }
    }
}