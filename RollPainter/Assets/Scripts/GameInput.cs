using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{
    LevelManager levelManager;
    CubeRoll cubeRoll;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        cubeRoll = FindObjectOfType<CubeRoll>();
    }

    void Update()
    {
        //// R 键重新开始
        if (Input.GetKeyDown(KeyCode.R))
        {
            levelManager.RestartLevel();
        }

        // Z 键撤销
        if (Input.GetKeyDown(KeyCode.Z))
        {
            cubeRoll.UndoMove();
        }

        // ESC 返回选关
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LevelSelectScene");
        }
    }
}