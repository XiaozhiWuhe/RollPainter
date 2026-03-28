using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
    public void StartLevel(int index)
    {
        PlayerPrefs.SetInt("LevelIndex", index);

        SceneManager.LoadScene("GameScene");
    }
}