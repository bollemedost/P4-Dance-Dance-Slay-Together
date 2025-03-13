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
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ensure it gets the component
        if (spriteRenderer == null)
        {
            Debug.LogError("❌ SpriteRenderer is missing on " + gameObject.name);
        }
    }

    void OnEnable()
    {
        // Subscribe to Arduino inputs
        ArduinoSerial.OnLeftPressed += HandleLeftPress;
        ArduinoSerial.OnDownPressed += HandleDownPress;
        ArduinoSerial.OnUpPressed += HandleUpPress;
        ArduinoSerial.OnRightPressed += HandleRightPress;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        ArduinoSerial.OnLeftPressed -= HandleLeftPress;
        ArduinoSerial.OnDownPressed -= HandleDownPress;
        ArduinoSerial.OnUpPressed -= HandleUpPress;
        ArduinoSerial.OnRightPressed -= HandleRightPress;
    }

    void HandleLeftPress()
    {
        if (keyToPress == KeyCode.LeftArrow) ShowPressedSprite();
    }

    void HandleDownPress()
    {
        if (keyToPress == KeyCode.DownArrow) ShowPressedSprite();
    }

    void HandleUpPress()
    {
        if (keyToPress == KeyCode.UpArrow) ShowPressedSprite();
    }

    void HandleRightPress()
    {
        if (keyToPress == KeyCode.RightArrow) ShowPressedSprite();
    }

    void ShowPressedSprite()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("❌ SpriteRenderer is NULL when trying to change sprite!");
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