using UnityEngine;

[System.Serializable]
public class MoveState
{
    public Vector3 cubePosition;
    public Quaternion cubeRotation;

    public int[] faceColors;

    public Tile changedTile;
    public TileColor previousColor;
}