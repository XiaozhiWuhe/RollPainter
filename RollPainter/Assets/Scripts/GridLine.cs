using UnityEngine;

[ExecuteAlways]
public class GridLine : MonoBehaviour
{
    public float size = 1f;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.01f, 0), new Vector3(size, 0, size));
    }
}