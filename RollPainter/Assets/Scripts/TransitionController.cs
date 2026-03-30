using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public static TransitionController Instance;

    public Image panel;

    public float duration = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetAlpha(0);
    }

    void SetAlpha(float a)
    {
        Color c = panel.color;
        c.a = a;
        panel.color = c;
    }

    public IEnumerator FadeIn()
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(0, 1, t / duration);

            SetAlpha(a);

            yield return null;
        }

        SetAlpha(1);
    }

    public IEnumerator FadeOut()
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(1, 0, t / duration);

            SetAlpha(a);

            yield return null;
        }

        SetAlpha(0);
    }
}