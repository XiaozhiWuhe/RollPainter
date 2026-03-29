using UnityEngine;
using UnityEngine.UI;

public class UIGoalBoard : MonoBehaviour
{
    public GameObject cellPrefab;   // 预制体：一个 UI Image 对象
    public GridLayoutGroup gridLayout;

    private int currentWidth;
    private int currentHeight;

    // 颜色映射：TileColor -> Color（需要与游戏内材质颜色匹配）
    private Color[] tileColorToUIColor = new Color[]
    {
        Color.white,    // White
        new Color(1f, 216/255f, 0f),   // Yellow
        Color.red,      // Red
        new Color(1f, 0.5f, 0f), // Orange
        Color.green,    // Green
        new Color(0f, 122/255f, 1f)      // Blue
    };

    void Awake()
    {
        if (cellPrefab == null)
            CreateDefaultPrefab();
    }

    void CreateDefaultPrefab()
    {
        // 如果没有提供预制体，动态创建一个默认的 Image
        cellPrefab = new GameObject("GoalCell", typeof(RectTransform), typeof(Image));
        cellPrefab.transform.SetParent(transform);
        cellPrefab.SetActive(false);
        var rect = cellPrefab.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(30, 30);
        // 添加一个边框效果（可选）
        var outline = cellPrefab.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1, -1);
    }

    public void Generate(LevelData data)
    {
        // 清除旧格子
        Clear();

        currentWidth = data.width;
        currentHeight = data.height;

        // 设置 GridLayoutGroup 的列数（如果使用动态列数）
        if (gridLayout != null)
        {
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = currentWidth;
        }

        // 生成所有格子
        for (int z = currentHeight - 1; z >= 0; z--)
        {
            for (int x = 0; x < currentWidth; x++)
            {
                TileColor color = data.GetColor(x, z);
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.SetActive(true);
                Image img = cell.GetComponent<Image>();
                if (img != null)
                {
                    if (color == TileColor.None)
                        img.color = new Color(211 / 255f, 211 / 255f, 211 / 255f);      // 无色格显示灰色
                    else
                        img.color = tileColorToUIColor[(int)color];
                }
                // 可以添加 Tooltip 或边框高亮
            }
        }

        // 调整 Panel 自身大小以适应内容（可选）
        AdjustPanelSize();
    }

    void Clear()
    {
        // 删除所有子物体（保留预制体引用，但这里简单删除所有）
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject != cellPrefab) // 不删除预制体源
                DestroyImmediate(child.gameObject);
        }
    }

    void AdjustPanelSize()
    {
        if (gridLayout == null) return;
        // 计算 Panel 应该的大小
        float totalWidth = currentWidth * (gridLayout.cellSize.x + gridLayout.spacing.x) - gridLayout.spacing.x + gridLayout.padding.left + gridLayout.padding.right;
        float totalHeight = currentHeight * (gridLayout.cellSize.y + gridLayout.spacing.y) - gridLayout.spacing.y + gridLayout.padding.top + gridLayout.padding.bottom;
        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }
}