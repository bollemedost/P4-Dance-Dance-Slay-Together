using UnityEngine;
using TMPro;

public class InfoPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public RectTransform imageLeft;
    public RectTransform imageRight;
    public TMP_InputField teamNameInput;
    public GameManager gameManager;

    public float delayBeforePopup = 1f;
    public float displayDuration = 3f;
    public float slideDuration = 1f;

    private Vector2 imageLeftStartPos;
    private Vector2 imageRightStartPos;
    private Vector2 imageLeftTargetPos;
    private Vector2 imageRightTargetPos;

    private void Start()
    {
        imageLeftTargetPos = imageLeft.anchoredPosition;
        imageRightTargetPos = imageRight.anchoredPosition;

        imageLeftStartPos = imageLeftTargetPos + new Vector2(-1000, 0);
        imageRightStartPos = imageRightTargetPos + new Vector2(1000, 0);

        imageLeft.anchoredPosition = imageLeftStartPos;
        imageRight.anchoredPosition = imageRightStartPos;

        gameManager.startPlaying = false;
        StartCoroutine(ShowPopupWithAnimation());
    }

    private System.Collections.IEnumerator ShowPopupWithAnimation()
    {
        popupPanel.SetActive(false);
        yield return new WaitForSeconds(delayBeforePopup);

        popupPanel.SetActive(true);
        yield return StartCoroutine(SlideImages(imageLeftStartPos, imageLeftTargetPos, imageRightStartPos, imageRightTargetPos));
        yield return new WaitForSeconds(displayDuration);
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

    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }

    public void StartGame()
    {
        if (teamNameInput == null || gameManager == null)
        {
            Debug.LogError("Missing references! Assign teamNameInput and gameManager in the Inspector.");
            return;
        }

        string teamName = teamNameInput.text.Trim();

        if (string.IsNullOrEmpty(teamName))
        {
            Debug.LogWarning("Please enter a team name before starting the game.");
            return;
        }

        PlayerPrefs.SetString("LastTeamName", teamName);

        popupPanel.SetActive(false);
        gameManager.startPlaying = true;
        gameManager.SetGameStartTime();
        gameManager.BeginGameplay();
    }
}
