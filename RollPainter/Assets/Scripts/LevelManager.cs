using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelDatabase levelDatabase;

    public GridGenerator gridGenerator;

    public GameObject cube;

    public GameObject winUI;

    public TMPro.TextMeshProUGUI stepText;

    Tile[] tiles;

    int currentLevelIndex = 0;

    int stepCount = 0;

    public LevelData CurrentLevelData { get; private set; }

    public bool levelFinished = false;

    void Start()
    {
        int levelIndex = PlayerPrefs.GetInt("LevelIndex", 0);

        LoadLevel(levelIndex);
    }

    public void LoadLevel(int index)
    {
        CurrentLevelData = levelDatabase.levels[index];

        LevelData data = CurrentLevelData;

        levelFinished = false;

        currentLevelIndex = index;

        stepCount = 0;

        UpdateStepUI();

        gridGenerator.size = data.width;

        gridGenerator.Generate();

        tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            tile.Paint(TileColor.None);
            tile.targetColor = data.GetColor(tile.x, tile.z);
        }

        CubeRoll cubeRoll = cube.GetComponent<CubeRoll>();
        Vector3 startPos = new Vector3(data.startPosition.x, 0.5f, data.startPosition.y);
        cubeRoll.ResetToInitial(startPos, Quaternion.identity);

        cube.transform.position = startPos;

        cube.GetComponent<CubeRoll>().enabled = true;

        winUI.SetActive(false);
    }

    public void AddStep()
    {
        stepCount++;

        UpdateStepUI();
    }

    public void ReduceStep()
    {
        stepCount = Mathf.Max(0, stepCount - 1);

        UpdateStepUI();
    }

    void UpdateStepUI()
    {
        if (stepText != null)
            stepText.text = "Steps: " + stepCount;
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);

        CubeRoll cube = FindObjectOfType<CubeRoll>();

        if (cube != null)
            cube.ClearHistory();
    }

    public void NextLevel()
    {
        int next = currentLevelIndex + 1;

        if (next < levelDatabase.levels.Length)
            LoadLevel(next);
    }

    public void CheckLevelComplete()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.targetColor != TileColor.None)
            {
                if (tile.currentColor != tile.targetColor)
                    return;
            }
        }

        LevelComplete();
    }

    void LevelComplete()
    {
        levelFinished = true;

        winUI.SetActive(true);

        cube.GetComponent<CubeRoll>().enabled = false;
    }
}