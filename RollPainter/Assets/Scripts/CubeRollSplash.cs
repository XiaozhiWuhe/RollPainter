using UnityEngine;

public class BlockRollColorSplash : MonoBehaviour
{
    [Header("喷溅粒子预设")]
    public GameObject splashPrefab;

    [Header("六个面的颜色")]
    public Color colorFront = Color.red;
    public Color colorBack = Color.blue;
    public Color colorLeft = Color.green;
    public Color colorRight = Color.yellow;
    public Color colorUp = Color.magenta;
    public Color colorDown = Color.cyan;

    [Header("判断静止阈值")]
    public float moveThreshold = 0.01f;

    private Vector3 lastPos;
    private bool wasMoving = false;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        bool isMoving = (transform.position - lastPos).magnitude > moveThreshold;

        if (wasMoving && !isMoving)
        {
            Color bottomColor = GetCurrentBottomFaceColor();
            SpawnSplash(bottomColor);
        }

        wasMoving = isMoving;
        lastPos = transform.position;
    }


    /// </summary>
    Color GetCurrentBottomFaceColor()
    {
        Vector3 worldDown = Vector3.down; // 关键：用世界向下判断！

        float dotFront = Vector3.Dot(transform.forward, worldDown);
        float dotBack = Vector3.Dot(-transform.forward, worldDown);
        float dotLeft = Vector3.Dot(-transform.right, worldDown);
        float dotRight = Vector3.Dot(transform.right, worldDown);
        float dotUp = Vector3.Dot(transform.up, worldDown);
        float dotDown = Vector3.Dot(-transform.up, worldDown);

        float max = Mathf.Max(dotFront, dotBack, dotLeft, dotRight, dotUp, dotDown);

        if (max == dotFront) return colorFront;
        if (max == dotBack) return colorBack;
        if (max == dotLeft) return colorLeft;
        if (max == dotRight) return colorRight;
        if (max == dotUp) return colorUp;
        if (max == dotDown) return colorDown;

        return Color.white;
    }

    void SpawnSplash(Color splashColor)
    {
        if (splashPrefab == null) return;

        GameObject splash = Instantiate(
            splashPrefab,
            transform.position,
            Quaternion.identity
        );

        ParticleSystem ps = splash.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startColor = splashColor;
        }

        Destroy(splash, ps.main.startLifetime.constant + 0.2f);
    }
}