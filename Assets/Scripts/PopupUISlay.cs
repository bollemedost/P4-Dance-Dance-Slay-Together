using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupUISlay : MonoBehaviour
{
    public CanvasGroup popupCanvasGroup;
    public float fadeDuration = 0.5f;

    void Start()
    {
        ShowPopup(); // Optional: show popup on scene load
    }

    public void ShowPopup()
    {
        StartCoroutine(FadeIn());
    }

    public void HidePopup()
    {
        popupCanvasGroup.alpha = 0f;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;
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
        popupCanvasGroup.alpha = 1f;
    }
}
