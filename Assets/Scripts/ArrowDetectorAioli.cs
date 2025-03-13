using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;

    // Feedback Prefabs (assigned in Unity Inspector)
    public GameObject normalHitText, goodHitText, perfectHitText, missText;
    public float feedbackDuration = 1f; // Time before feedback disappears

    private void OnEnable()
    {
        // Subscribe to Arduino inputs
        ArduinoSerial.OnLeftPressed += HandleLeftPress;
        ArduinoSerial.OnDownPressed += HandleDownPress;
        ArduinoSerial.OnUpPressed += HandleUpPress;
        ArduinoSerial.OnRightPressed += HandleRightPress;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        ArduinoSerial.OnLeftPressed -= HandleLeftPress;
        ArduinoSerial.OnDownPressed -= HandleDownPress;
        ArduinoSerial.OnUpPressed -= HandleUpPress;
        ArduinoSerial.OnRightPressed -= HandleRightPress;
    }

    void HandleLeftPress() { if (keyToPress == KeyCode.LeftArrow) CheckPress(); }
    void HandleDownPress() { if (keyToPress == KeyCode.DownArrow) CheckPress(); }
    void HandleUpPress() { if (keyToPress == KeyCode.UpArrow) CheckPress(); }
    void HandleRightPress() { if (keyToPress == KeyCode.RightArrow) CheckPress(); }

    void CheckPress()
    {
        if (canBePressed)
        {
            gameObject.SetActive(false); // Remove the arrow from screen

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

            // Destroy the feedback after delay
            if (feedbackInstance != null)
            {
                Destroy(feedbackInstance, feedbackDuration);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator" && gameObject.activeSelf)
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
