using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupUISlay : MonoBehaviour
{
    public CanvasGroup popupCanvasGroup;
    public float fadeDuration = 0.5f;

    public void ShowPopup()
    {
        StartCoroutine(FadeIn());
    }

    public void HidePopup()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        popupCanvasGroup.interactable = true;
        popupCanvasGroup.blocksRaycasts = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            popupCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        popupCanvasGroup.alpha = 1;
    }

    IEnumerator FadeOut()
    {
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            popupCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        popupCanvasGroup.alpha = 0;
    }
}
