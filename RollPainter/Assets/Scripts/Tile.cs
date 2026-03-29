using UnityEngine;

public enum TileColor
{
    None = -1,
    White = 0,
    Yellow = 1,
    Red = 2,
    Orange = 3,
    Green = 4,
    Blue = 5
}
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