using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;
    public int playerNumber; // NEW: 1 for Player 1, 2 for Player 2

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

    // PLAYER 1
    void HandleLeft1Press() { if (keyToPress == KeyCode.LeftArrow && playerNumber == 1) CheckPress(); }
    void HandleDown1Press() { if (keyToPress == KeyCode.DownArrow && playerNumber == 1) CheckPress(); }
    void HandleUp1Press() { if (keyToPress == KeyCode.UpArrow && playerNumber == 1) CheckPress(); }
    void HandleRight1Press() { if (keyToPress == KeyCode.RightArrow && playerNumber == 1) CheckPress(); }

    // PLAYER 2
    void HandleLeft2Press() { if (keyToPress == KeyCode.LeftArrow && playerNumber == 2) CheckPress(); }
    void HandleDown2Press() { if (keyToPress == KeyCode.DownArrow && playerNumber == 2) CheckPress(); }
    void HandleUp2Press() { if (keyToPress == KeyCode.UpArrow && playerNumber == 2) CheckPress(); }
    void HandleRight2Press() { if (keyToPress == KeyCode.RightArrow && playerNumber == 2) CheckPress(); }

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
                SoundManager.Instance.PlayPerfectSound();
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
        if ((other.CompareTag("Activator1") && playerNumber == 1) || (other.CompareTag("Activator2") && playerNumber == 2))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((other.CompareTag("Activator1") && playerNumber == 1) || (other.CompareTag("Activator2") && playerNumber == 2)) && gameObject.activeSelf)
        {
            canBePressed = false;
            Debug.Log("❌ Missed Note!");
            SoundManager.Instance.PlayMissSound();
            GameManager.instance.NoteMissed(gameObject);

            GameObject missInstance = Instantiate(missText, transform.position, Quaternion.identity);
            if (missInstance != null)
            {
                Destroy(missInstance, feedbackDuration);
            }
        }
    }

    // ekstra til test - wasd
    void Update()
    {
        if (!canBePressed) return;

        if (playerNumber == 1)
        {
            if (keyToPress == KeyCode.LeftArrow && Input.GetKeyDown(KeyCode.A)) CheckPress();
            if (keyToPress == KeyCode.DownArrow && Input.GetKeyDown(KeyCode.S)) CheckPress();
            if (keyToPress == KeyCode.UpArrow && Input.GetKeyDown(KeyCode.W)) CheckPress();
            if (keyToPress == KeyCode.RightArrow && Input.GetKeyDown(KeyCode.D)) CheckPress();
        }

        if (playerNumber == 2)
        {
            if (keyToPress == KeyCode.LeftArrow && Input.GetKeyDown(KeyCode.LeftArrow)) CheckPress();
            if (keyToPress == KeyCode.DownArrow && Input.GetKeyDown(KeyCode.DownArrow)) CheckPress();
            if (keyToPress == KeyCode.UpArrow && Input.GetKeyDown(KeyCode.UpArrow)) CheckPress();
            if (keyToPress == KeyCode.RightArrow && Input.GetKeyDown(KeyCode.RightArrow)) CheckPress();
        }
    }


}

