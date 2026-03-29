using UnityEngine;

public class MiniGoalBoard : MonoBehaviour
{
    public float cellSize = 0.2f;

    public Material defaultMaterial;
    public Material[] colorMaterials;

    public void Generate(LevelData data)
    {
        Clear();

        for (int x = 0; x < data.width; x++)
        {
            for (int z = 0; z < data.height; z++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Plane);

                cell.transform.parent = transform;
                cell.transform.localScale = Vector3.one * 0.02f;

                cell.transform.localPosition =
                    new Vector3(x * cellSize, 0, z * cellSize);

                Renderer r = cell.GetComponent<Renderer>();

                TileColor color = data.GetColor(x, z);

                if (color == TileColor.None)
                    r.material = defaultMaterial;
                else
                    r.material = colorMaterials[(int)color];
            }
        }

        CenterBoard(data);
    }

    void CenterBoard(LevelData data)
    {
        float offsetX = (data.width - 1) * cellSize / 2f;
        float offsetZ = (data.height - 1) * cellSize / 2f;

        transform.localPosition -= new Vector3(offsetX, 0, offsetZ);
    }

    void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}