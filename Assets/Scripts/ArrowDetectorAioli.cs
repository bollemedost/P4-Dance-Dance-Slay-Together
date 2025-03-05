using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Required for DDR Input System

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress; // Keyboard input
    private DDRInput ddrInput; // DDR Input reference

    public float normalHitThreshold = 0.25f;
    public float goodHitThreshold = 0.15f;

    // Prefabs for feedback (Assign in Unity Inspector)
    public GameObject normalHitText, goodHitText, perfectHitText, missText;
    public float delayTime = 1f; // Time before feedback disappears
    private GameObject spawnedText;

    private void Awake()
    {
        ddrInput = new DDRInput(); // Initialize DDR input
        ddrInput.Enable(); // Enable DDR inputs
    }

    void Update()
    {
        bool keyPressed = Input.GetKeyDown(keyToPress);

        // Check DDR pad inputs
        bool ddrPressed = false;
        switch (keyToPress)
        {
            case KeyCode.UpArrow:
                ddrPressed = ddrInput.DDR.Up.WasPressedThisFrame();
                break;
            case KeyCode.DownArrow:
                ddrPressed = ddrInput.DDR.Down.WasPressedThisFrame();
                break;
            case KeyCode.LeftArrow:
                ddrPressed = ddrInput.DDR.Left.WasPressedThisFrame();
                break;
            case KeyCode.RightArrow:
                ddrPressed = ddrInput.DDR.Right.WasPressedThisFrame();
                break;
        }

        // If key or DDR pad button is pressed
        if ((keyPressed || ddrPressed) && canBePressed)
        {
            gameObject.SetActive(false); // Remove the arrow from screen

            // Determine accuracy of hit
            if (Mathf.Abs(transform.position.y) > normalHitThreshold)
            {
                Debug.Log("Hit!");
                GameManager.instance.NormalHit();
                spawnedText = Instantiate(normalHitText, transform.position, Quaternion.identity);
            }
            else if (Mathf.Abs(transform.position.y) > goodHitThreshold)
            {
                Debug.Log("Good Hit!");
                GameManager.instance.GoodHit();
                spawnedText = Instantiate(goodHitText, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Perfect Hit!");
                GameManager.instance.PerfectHit();
                spawnedText = Instantiate(perfectHitText, transform.position, Quaternion.identity);
            }

            // Destroy the feedback after delay
            if (spawnedText != null)
            {
                Debug.Log($"🟢 Spawning Feedback: {spawnedText.name} at {transform.position}");
                Destroy(spawnedText, delayTime);
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
            Debug.Log("❌ Missed Note - Resetting Multiplier");
            GameManager.instance.NoteMissed(gameObject);

            // Spawn miss text feedback
            spawnedText = Instantiate(missText, transform.position, Quaternion.identity);
            if (spawnedText != null)
            {
                Destroy(spawnedText, delayTime);
            }
        }
    }
}
