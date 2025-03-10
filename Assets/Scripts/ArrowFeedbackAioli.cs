using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Required for DDR Input System

public class ArrowFeedbackAioli : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public KeyCode keyToPress; // Keyboard key

    private DDRInput ddrInput; // DDR Input reference
    public string ddrActionName; // The action name assigned to this arrow (set in Unity Inspector)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize DDR input system
        ddrInput = new DDRInput();
        ddrInput.Enable();
    }

    void Update()
    {
        // Check for keyboard key press
        bool keyPressed = Input.GetKeyDown(keyToPress);
        bool keyReleased = Input.GetKeyUp(keyToPress);

        // Check for specific DDR input assigned to this arrow
        bool ddrPressed = false;
        bool ddrReleased = false;

        switch (ddrActionName)
        {
            case "Up":
                ddrPressed = ddrInput.DDR.Up.WasPressedThisFrame();
                ddrReleased = ddrInput.DDR.Up.WasReleasedThisFrame();
                break;
            case "Down":
                ddrPressed = ddrInput.DDR.Down.WasPressedThisFrame();
                ddrReleased = ddrInput.DDR.Down.WasReleasedThisFrame();
                break;
            case "Left":
                ddrPressed = ddrInput.DDR.Left.WasPressedThisFrame();
                ddrReleased = ddrInput.DDR.Left.WasReleasedThisFrame();
                break;
            case "Right":
                ddrPressed = ddrInput.DDR.Right.WasPressedThisFrame();
                ddrReleased = ddrInput.DDR.Right.WasReleasedThisFrame();
                break;
        }

        // Change sprite when key or assigned DDR button is pressed
        if (keyPressed || ddrPressed)
        {
            spriteRenderer.sprite = pressedSprite;
        }

        // Revert sprite when key or assigned DDR button is released
        if (keyReleased || ddrReleased)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}

//References: Used https://www.youtube.com/@gamesplusjames as a reference for the code.