using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int z;

    private Renderer rend;

    public Material defaultMaterial;
    public Material[] colorMaterials;

    public TileColor currentColor = TileColor.None;
    public TileColor targetColor = TileColor.None;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        if (defaultMaterial != null)
            rend.sharedMaterial = defaultMaterial;
    }

    public void Paint(TileColor color)
    {
        currentColor = color;

        if (color == TileColor.None)
        {
            rend.sharedMaterial = defaultMaterial;
            return;
        }

        int index = (int)color;

        if (index >= 0 && index < colorMaterials.Length)
        {
            rend.sharedMaterial = colorMaterials[index];
        }
    }
}