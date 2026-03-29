using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    LevelData data;

    public override void OnInspectorGUI()
    {
        data = (LevelData)target;

        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Initialize Grid"))
        {
            data.Initialize();
        }

        if (data.targetColors == null)
            return;

        GUILayout.Space(10);
        GUILayout.Label("Level Pattern");

        for (int z = data.height - 1; z >= 0; z--)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < data.width; x++)
            {
                TileColor color = data.GetColor(x, z);

                TileColor newColor =
                    (TileColor)EditorGUILayout.EnumPopup(color, GUILayout.Width(70));

                if (newColor != color)
                {
                    data.SetColor(x, z, newColor);
                    EditorUtility.SetDirty(data);
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}