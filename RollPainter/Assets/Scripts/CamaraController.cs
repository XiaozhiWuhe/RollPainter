using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GridGenerator grid;

    public float heightMultiplier = 2f;
    public float horizontalOffset = 1f;

    void Start()
    {
        if (grid == null)
            grid = FindObjectOfType<GridGenerator>();
    }

    void LateUpdate()
    {
        if (grid == null) return;

        float centerX = (grid.size - 1) * grid.cellSize * 0.5f+horizontalOffset;
        float centerZ = (grid.size - 1) * grid.cellSize * 0.5f;

        float height = grid.size * heightMultiplier;

        Vector3 center = new Vector3(centerX, 0, centerZ);

        transform.position = new Vector3(center.x, height, center.z);

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}