using System.Collections;
using UnityEngine;

public class LevelCompleteAnimator : MonoBehaviour
{
    public float delayStep = 0.05f;
    public float explodeForce = 4f;
    public float cubeSize = 0.3f;

    public IEnumerator Play(System.Action onFinish)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        int centerX = 0;
        int centerZ = 0;

        foreach (Tile t in tiles)
        {
            centerX += t.x;
            centerZ += t.z;
        }

        centerX /= tiles.Length;
        centerZ /= tiles.Length;

        int maxDist = 0;

        foreach (Tile t in tiles)
        {
            int dist = Mathf.Abs(t.x - centerX) + Mathf.Abs(t.z - centerZ);

            if (dist > maxDist)
                maxDist = dist;

            StartCoroutine(ExplodeTile(t, dist));
        }

        yield return new WaitForSeconds(maxDist * delayStep + 1f);

        onFinish?.Invoke();
    }

    IEnumerator ExplodeTile(Tile tile, int dist)
    {
        yield return new WaitForSeconds(dist * delayStep);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = tile.transform.position + Vector3.up * 0.5f;
        cube.transform.localScale = Vector3.one * cubeSize;

        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        Renderer tileRenderer = tile.GetComponent<Renderer>();

        cubeRenderer.material = tileRenderer.material;

        Rigidbody rb = cube.AddComponent<Rigidbody>();

        Vector3 dir = new Vector3(tile.x, 0, tile.z).normalized;

        rb.AddForce((dir + Vector3.up) * explodeForce, ForceMode.Impulse);

        Destroy(cube, 3f);
    }
}