using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    LevelData level;

    TileColor paintColor = TileColor.Red;

    public override void OnInspectorGUI()
    {
        level = (LevelData)target;

        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Initialize Grid"))
        {
            level.Initialize();
        }

        GUILayout.Space(10);

        paintColor =
            (TileColor)EditorGUILayout.EnumPopup("Paint Color", paintColor);

        GUILayout.Space(10);

        DrawGrid();
    }

    void DrawGrid()
    {
        if (level.targetColors == null)
            return;

        for (int z = level.height - 1; z >= 0; z--)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < level.width; x++)
            {
                TileColor color = level.GetColor(x, z);

                GUI.backgroundColor = GetColor(color);

                if (GUILayout.Button("", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    level.SetColor(x, z, paintColor);
                }
            }

            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }

    Color GetColor(TileColor c)
    {
        switch (c)
        {
            case TileColor.White: return Color.white;
            case TileColor.Yellow: return Color.yellow;
            case TileColor.Red: return Color.red;
            case TileColor.Orange: return new Color(1f, 0.5f, 0f);
            case TileColor.Green: return Color.green;
            case TileColor.Blue: return Color.blue;

            default: return Color.gray;
        }
    }
}