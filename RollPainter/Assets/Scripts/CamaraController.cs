using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GridGenerator grid;          // 网格生成器引用
    public float heightMultiplier = 1.2f; // 高度系数（相对于网格最大尺寸）

    void Start()
    {
        if (grid == null)
            grid = FindObjectOfType<GridGenerator>();
    }

    void LateUpdate()
    {
        if (grid == null) return;

        // 计算网格的世界中心（假设格子左下角为原点）
        float centerX = (grid.width - 1) * grid.cellSize * 0.5f;
        float centerZ = (grid.height - 1) * grid.cellSize * 0.5f;

        // 动态计算高度：基于网格最大跨度，确保相机能容纳整个网格
        float maxSpan = Mathf.Max(grid.width, grid.height) * grid.cellSize;
        float height = maxSpan * heightMultiplier;

        // 设置相机位置：正上方俯视
        transform.position = new Vector3(centerX, height, centerZ);
        // 俯视旋转：绕 X 轴 -90° 或 90°，使相机向下看
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}