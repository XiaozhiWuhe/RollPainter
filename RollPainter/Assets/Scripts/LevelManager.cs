using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelCompleteAnimator completeAnimator;

    public GameObject winUI;

    public UIGoalBoard uiGoalBoard;

    public LevelDatabase levelDatabase;

    public GridGenerator gridGenerator;

    public GameObject cube;

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
        CurrentLevelData.Initialize();
        LevelData data = CurrentLevelData;

        levelFinished = false;

        currentLevelIndex = index;

        stepCount = 0;

        UpdateStepUI();

        gridGenerator.width = data.width;
        gridGenerator.height = data.height;

        gridGenerator.Generate();
        uiGoalBoard.Generate(data);

        tiles = gridGenerator.tiles.Cast<Tile>().ToArray();

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
            if (tile.targetColor != TileColor.None && tile.currentColor != tile.targetColor)
            {
                return;
            }
        }
        LevelComplete();
    }

    void LevelComplete()
    {
        levelFinished = true;

        StartCoroutine(
            completeAnimator.Play(ShowWinUI)
        );
    }

    void ShowWinUI()
    {
        winUI.SetActive(true);
    }
}