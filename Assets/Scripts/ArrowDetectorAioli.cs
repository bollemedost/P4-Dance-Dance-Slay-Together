 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed; 
    public KeyCode keyToPress; //set in inspector

    public float normalHitThreshold = 0.25f;
    public float goodHitThreshold = 0.15f;

    void Update() 
    {
        if(Input.GetKeyDown(keyToPress))
        {
            if(canBePressed)
            {
                gameObject.SetActive(false); //destroy arrow if pressed 

                if(Mathf.Abs(transform.position.y) > normalHitThreshold)
                {
                    Debug.Log("Hit");
                    GameManager.instance.NormalHit(); //call NoteHit function from GameManager; tells the gameManager that the note/arrow was hit
                }
                else if(Mathf.Abs(transform.position.y) > goodHitThreshold)
                {
                    Debug.Log("Good Hit");
                    GameManager.instance.GoodHit(); //call NoteHit function from GameManager; tells the gameManager that the note/arrow was hit
                }
                else
                {
                    Debug.Log("Perfect Hit");
                    GameManager.instance.PerfectHit(); //call NoteHit function from GameManager; tells the gameManager that the note/arrow was hit
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //if arrow is in the activator zone
    {
        if(other.tag == "Activator")
        {
            canBePressed = true; //arrow can be pressed

        }
    }
    private void OnTriggerExit2D(Collider2D other) //if arrow is not in the activator zone
    {
        if(other.tag == "Activator" && gameObject.activeSelf)
        {
            canBePressed = false; //arrow cannot be pressed}

            GameManager.instance.NoteMissed(); //call NoteMissed function from GameManager; tells the gameManager that the note/arrow was missed
        }
    }
}

//References: Used https://www.youtube.com/@gamesplusjames as a reference for the code.