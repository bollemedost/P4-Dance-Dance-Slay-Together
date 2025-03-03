using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectorAioli : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress; // Set in Inspector

    public float normalHitThreshold = 0.25f;
    public float goodHitThreshold = 0.15f;

    // Text pop-up
    public GameObject normalHitText, goodHitText, perfectHitText, missText;
    public float delayTime = 1f;
    private GameObject spawnedText;

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false); // Destroy arrow if pressed 

                if (Mathf.Abs(transform.position.y) > normalHitThreshold)
                {
                    Debug.Log("Hit");
                    GameManager.instance.NormalHit();
                    spawnedText = Instantiate(normalHitText, transform.position, normalHitText.transform.rotation);
                    Destroy(spawnedText, delayTime);
                }
                else if (Mathf.Abs(transform.position.y) > goodHitThreshold)
                {
                    Debug.Log("Good Hit");
                    GameManager.instance.GoodHit();
                    spawnedText = Instantiate(goodHitText, transform.position, goodHitText.transform.rotation);
                    Destroy(spawnedText, delayTime);
                }
                else
                {
                    Debug.Log("Perfect Hit");
                    GameManager.instance.PerfectHit();
                    spawnedText = Instantiate(perfectHitText, transform.position, perfectHitText.transform.rotation);
                    Destroy(spawnedText, delayTime);
                }
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

            // Pass THIS arrow to NoteMissed so only it gets destroyed
            GameManager.instance.NoteMissed(gameObject);

            spawnedText = Instantiate(missText, transform.position, missText.transform.rotation);
            Destroy(spawnedText, delayTime);
        }
    }
}


//References: Used https://www.youtube.com/@gamesplusjames as a reference for the code.