using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "RollPainter/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;

    public int width = 5;
    public int height = 5;

    public Vector2Int startPosition;

    // 用一维数组存储，但按二维逻辑使用
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

    int GetIndex(int x, int z)
    {
        return z * width + x;
    }

    public TileColor GetColor(int x, int z)
    {
        int index = GetIndex(x, z);

        if (index < 0 || index >= targetColors.Length)
            return TileColor.None;

        return targetColors[index];
    }

    public void SetColor(int x, int z, TileColor color)
    {
        int index = GetIndex(x, z);

        if (index >= 0 && index < targetColors.Length)
            targetColors[index] = color;
    }
}