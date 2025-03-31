using UnityEngine;

public class InfoPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public RectTransform imageLeft;
    public RectTransform imageRight;

    public float delayBeforePopup = 1f;
    public float displayDuration = 3f;
    public float slideDuration = 1f;

    private Vector2 imageLeftStartPos;
    private Vector2 imageRightStartPos;
    private Vector2 imageLeftTargetPos;
    private Vector2 imageRightTargetPos;

    private void Start()
    {
        // Store final target positions
        imageLeftTargetPos = imageLeft.anchoredPosition;
        imageRightTargetPos = imageRight.anchoredPosition;

        // Set initial positions off-screen
        imageLeftStartPos = imageLeftTargetPos + new Vector2(-1000, 0);
        imageRightStartPos = imageRightTargetPos + new Vector2(1000, 0);

        imageLeft.anchoredPosition = imageLeftStartPos;
        imageRight.anchoredPosition = imageRightStartPos;

        StartCoroutine(ShowPopupWithAnimation());
    }

    private System.Collections.IEnumerator ShowPopupWithAnimation()
    {
        popupPanel.SetActive(false);
        yield return new WaitForSeconds(delayBeforePopup);

        popupPanel.SetActive(true);

        // Slide in
        yield return StartCoroutine(SlideImages(imageLeftStartPos, imageLeftTargetPos, imageRightStartPos, imageRightTargetPos));

        // Wait while showing
        yield return new WaitForSeconds(displayDuration);

        // Slide out
        yield return StartCoroutine(SlideImages(imageLeftTargetPos, imageLeftStartPos, imageRightTargetPos, imageRightStartPos));

        popupPanel.SetActive(false);
    }

    private System.Collections.IEnumerator SlideImages(Vector2 leftFrom, Vector2 leftTo, Vector2 rightFrom, Vector2 rightTo)
    {
        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / slideDuration);
            float eased = EaseInOutQuad(progress);

            imageLeft.anchoredPosition = Vector2.Lerp(leftFrom, leftTo, eased);
            imageRight.anchoredPosition = Vector2.Lerp(rightFrom, rightTo, eased);

            yield return null;
        }
    }

    // Easing function for smooth animation
    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}
