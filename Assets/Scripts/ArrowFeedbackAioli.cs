using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFeedbackAioli : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    public KeyCode keyToPress;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            spriteRenderer.sprite = pressedSprite;
        }
        if(Input.GetKeyUp(keyToPress))
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}


//References: Used https://www.youtube.com/@gamesplusjames as a reference for the code.