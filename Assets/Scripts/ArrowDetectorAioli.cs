using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;

    // Feedback Prefabs
    public GameObject normalHitText, goodHitText, perfectHitText, missText;
    public float feedbackDuration = 1f;

    private void OnEnable()
    {
        // Player 1
        ArduinoSerial.OnLeft1Pressed += HandleLeft1Press;
        ArduinoSerial.OnDown1Pressed += HandleDown1Press;
        ArduinoSerial.OnUp1Pressed += HandleUp1Press;
        ArduinoSerial.OnRight1Pressed += HandleRight1Press;

        // Player 2
        ArduinoSerial.OnLeft2Pressed += HandleLeft2Press;
        ArduinoSerial.OnDown2Pressed += HandleDown2Press;
        ArduinoSerial.OnUp2Pressed += HandleUp2Press;
        ArduinoSerial.OnRight2Pressed += HandleRight2Press;
    }

    private void OnDisable()
    {
        // Player 1
        ArduinoSerial.OnLeft1Pressed -= HandleLeft1Press;
        ArduinoSerial.OnDown1Pressed -= HandleDown1Press;
        ArduinoSerial.OnUp1Pressed -= HandleUp1Press;
        ArduinoSerial.OnRight1Pressed -= HandleRight1Press;

        // Player 2
        ArduinoSerial.OnLeft2Pressed -= HandleLeft2Press;
        ArduinoSerial.OnDown2Pressed -= HandleDown2Press;
        ArduinoSerial.OnUp2Pressed -= HandleUp2Press;
        ArduinoSerial.OnRight2Pressed -= HandleRight2Press;
    }

    // Player 1
    void HandleLeft1Press() { if (keyToPress == KeyCode.LeftArrow) CheckPress(); }
    void HandleDown1Press() { if (keyToPress == KeyCode.DownArrow) CheckPress(); }
    void HandleUp1Press() { if (keyToPress == KeyCode.UpArrow) CheckPress(); }
    void HandleRight1Press() { if (keyToPress == KeyCode.RightArrow) CheckPress(); }

    // Player 2
    void HandleLeft2Press() { if (keyToPress == KeyCode.LeftArrow) CheckPress(); }
    void HandleDown2Press() { if (keyToPress == KeyCode.DownArrow) CheckPress(); }
    void HandleUp2Press() { if (keyToPress == KeyCode.UpArrow) CheckPress(); }
    void HandleRight2Press() { if (keyToPress == KeyCode.RightArrow) CheckPress(); }

    void CheckPress()
    {
        if (canBePressed)
        {
            gameObject.SetActive(false); // Remove arrow

            float hitAccuracy = Mathf.Abs(transform.position.y);

            GameObject feedbackInstance = null;

            if (hitAccuracy > 0.25f)
            {
                Debug.Log("✅ Normal Hit!");
                GameManager.instance.NormalHit();
                feedbackInstance = Instantiate(normalHitText, transform.position, Quaternion.identity);
            }
            else if (hitAccuracy > 0.15f)
            {
                Debug.Log("🌟 Good Hit!");
                GameManager.instance.GoodHit();
                feedbackInstance = Instantiate(goodHitText, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("💯 Perfect Hit!");
                GameManager.instance.PerfectHit();
                feedbackInstance = Instantiate(perfectHitText, transform.position, Quaternion.identity);
            }

            if (feedbackInstance != null)
            {
                Destroy(feedbackInstance, feedbackDuration);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator1") || other.CompareTag("Activator2"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Activator1") || other.CompareTag("Activator2")) && gameObject.activeSelf)
        {
            canBePressed = false;
            Debug.Log("❌ Missed Note!");
            GameManager.instance.NoteMissed(gameObject);

            GameObject missInstance = Instantiate(missText, transform.position, Quaternion.identity);
            if (missInstance != null)
            {
                Destroy(missInstance, feedbackDuration);
            }
        }
    }
}
