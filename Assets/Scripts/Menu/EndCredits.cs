using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndCredits : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text uiText;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float duration;

    public void AnimateOpacity(float targetOpacity)
    {
        if (uiText != null)
        {
            StartCoroutine(FadeTextOpacity(uiText, targetOpacity, duration));
        }
        else if (textMeshPro != null)
        {
            StartCoroutine(FadeTextMeshProOpacity(textMeshPro, targetOpacity, duration));
        }
    }

    private IEnumerator FadeTextOpacity(Text text, float targetOpacity, float duration)
    {
        Color color = text.color;
        float startOpacity = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / duration);
            text.color = color;
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        color.a = targetOpacity;
        text.color = color;
    }

    private IEnumerator FadeTextMeshProOpacity(TextMeshProUGUI tmp, float targetOpacity, float duration)
    {
        Color color = tmp.color;
        float startOpacity = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / duration);
            tmp.color = color;
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        color.a = targetOpacity;
        tmp.color = color;
    }
}
