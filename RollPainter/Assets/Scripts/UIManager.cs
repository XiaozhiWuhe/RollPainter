using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
}