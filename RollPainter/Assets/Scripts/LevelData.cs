using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "RollPainter/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;

    public int width = 5;
    public int height = 5;

    public Vector2Int startPosition;

    public TileColor[] targetColors;

    public void Initialize()
    {
        int size = width * height;

        if (targetColors == null || targetColors.Length != size)
        {
            targetColors = new TileColor[size];

            for (int i = 0; i < size; i++)
                targetColors[i] = TileColor.None;
        }
    }

    public TileColor GetColor(int x, int z)
    {
        int index = z * width + x;

        if (index < 0 || index >= targetColors.Length)
            return TileColor.None;

        return targetColors[index];
    }

    public void SetColor(int x, int z, TileColor color)
    {
        int index = z * width + x;

        if (index >= 0 && index < targetColors.Length)
            targetColors[index] = color;
    }
}