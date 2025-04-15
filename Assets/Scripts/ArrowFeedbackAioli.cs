using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFeedbackAioli : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public KeyCode keyToPress;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is missing on " + gameObject.name);
        }
    }

    void OnEnable()
    {
        // Player 1 inputs
        ArduinoSerial.OnLeft1Pressed += HandleLeft1Press;
        ArduinoSerial.OnDown1Pressed += HandleDown1Press;
        ArduinoSerial.OnUp1Pressed += HandleUp1Press;
        ArduinoSerial.OnRight1Pressed += HandleRight1Press;

        // Player 2 inputs
        ArduinoSerial.OnLeft2Pressed += HandleLeft2Press;
        ArduinoSerial.OnDown2Pressed += HandleDown2Press;
        ArduinoSerial.OnUp2Pressed += HandleUp2Press;
        ArduinoSerial.OnRight2Pressed += HandleRight2Press;
    }

    void OnDisable()
    {
        // Player 1 inputs
        ArduinoSerial.OnLeft1Pressed -= HandleLeft1Press;
        ArduinoSerial.OnDown1Pressed -= HandleDown1Press;
        ArduinoSerial.OnUp1Pressed -= HandleUp1Press;
        ArduinoSerial.OnRight1Pressed -= HandleRight1Press;

        // Player 2 inputs
        ArduinoSerial.OnLeft2Pressed -= HandleLeft2Press;
        ArduinoSerial.OnDown2Pressed -= HandleDown2Press;
        ArduinoSerial.OnUp2Pressed -= HandleUp2Press;
        ArduinoSerial.OnRight2Pressed -= HandleRight2Press;
    }

    // Player 1 handlers
    void HandleLeft1Press() { if (keyToPress == KeyCode.LeftArrow && CompareTag("Activator1")) ShowPressedSprite(); }
    void HandleDown1Press() { if (keyToPress == KeyCode.DownArrow && CompareTag("Activator1")) ShowPressedSprite(); }
    void HandleUp1Press() { if (keyToPress == KeyCode.UpArrow && CompareTag("Activator1")) ShowPressedSprite(); }
    void HandleRight1Press() { if (keyToPress == KeyCode.RightArrow && CompareTag("Activator1")) ShowPressedSprite(); }

    // Player 2 handlers
    void HandleLeft2Press() { if (keyToPress == KeyCode.LeftArrow && CompareTag("Activator2")) ShowPressedSprite(); }
    void HandleDown2Press() { if (keyToPress == KeyCode.DownArrow && CompareTag("Activator2")) ShowPressedSprite(); }
    void HandleUp2Press() { if (keyToPress == KeyCode.UpArrow && CompareTag("Activator2")) ShowPressedSprite(); }
    void HandleRight2Press() { if (keyToPress == KeyCode.RightArrow && CompareTag("Activator2")) ShowPressedSprite(); }

    void ShowPressedSprite()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is NULL when trying to change sprite!");
            return;
        }

        spriteRenderer.sprite = pressedSprite;
        Invoke("ResetSprite", 0.1f);
    }

    void ResetSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}



//References: Used https://www.youtube.com/@gamesplusjames as a reference for the code.