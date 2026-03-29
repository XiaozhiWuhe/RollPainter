using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public float cellSize = 1f;
    public Tile[,] tiles;

    public Material gridMaterial;
    public Material defaultMaterial;
    public Material[] colorMaterials;

    public void Generate()
    {
        Clear();
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Plane);

                cell.transform.parent = transform;
                cell.transform.position = new Vector3(x * cellSize, 0, z * cellSize);
                cell.transform.localScale = Vector3.one * 0.1f;

                cell.name = $"Tile_{x}_{z}";

                Renderer r = cell.GetComponent<Renderer>();

                if (gridMaterial != null)
                    r.material = gridMaterial;

                Tile tile = cell.AddComponent<Tile>();

                tile.x = x;
                tile.z = z;
                tile.defaultMaterial = defaultMaterial;
                tile.colorMaterials = colorMaterials;

                tiles[x, z] = tile;
            }
        }
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
#if UNITY_EDITOR
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
        }
    }
}