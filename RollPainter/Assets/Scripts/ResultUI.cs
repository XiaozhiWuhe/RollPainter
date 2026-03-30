using UnityEngine;
using TMPro;
using System.Collections;

public class ResultUI : MonoBehaviour
{
    public static ResultUI Instance;

    public GameObject panel;
    public TextMeshProUGUI stepText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(int steps)
    {
        StartCoroutine(ShowRoutine(steps));
    }

    IEnumerator ShowRoutine(int steps)
    {
        panel.SetActive(true);

        stepText.text = "Level Complete!";

        yield return new WaitForSeconds(2f);

        stepText.text = "Steps Used";

        yield return new WaitForSeconds(2f);

        stepText.text = steps.ToString();
    }
}