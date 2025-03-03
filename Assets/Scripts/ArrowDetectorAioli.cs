  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed; 
    public KeyCode keyToPress; //set in inspector

    public float normalHitThreshold = 0.25f;
    public float goodHitThreshold = 0.15f;

    // Text pop-up
    public GameObject normalHitText, goodHitText, perfectHitText, missText; //when it is the same type, you can create the variables together seperated by commas. 
    public float delayTime = 1f;
    private GameObject spawnedText;

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
                    spawnedText = Instantiate(normalHitText, transform.position, normalHitText.transform.rotation); //make hit text pop up when hit is detected
                    Destroy(spawnedText, delayTime);
                }
                else if(Mathf.Abs(transform.position.y) > goodHitThreshold)
                {
                    Debug.Log("Good Hit");
                    GameManager.instance.GoodHit(); //call NoteHit function from GameManager; tells the gameManager that the note/arrow was hit
                    spawnedText = Instantiate(goodHitText, transform.position, goodHitText.transform.rotation); //make hit text pop up when hit is detected
                    Destroy(spawnedText, delayTime);
                }
                else
                {
                    Debug.Log("Perfect Hit");
                    GameManager.instance.PerfectHit(); //call NoteHit function from GameManager; tells the gameManager that the note/arrow was hit
                    spawnedText = Instantiate(perfectHitText, transform.position, perfectHitText.transform.rotation); //make hit text pop up when hit is detected
                    Destroy(spawnedText, delayTime);
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
            spawnedText = Instantiate(missText, transform.position, missText.transform.rotation); //make miss text pop up when hit is detected
            Destroy(spawnedText, delayTime);
        }
    }
}

//References: Used https://www.youtube.com/@gamesplusjames as a reference for the code.