 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScrollerAioli : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;
    void Start()
    {
        beatTempo = beatTempo / 60f; //how fast it/arrows should move per second
    }

    void Update()
    {
        if(!hasStarted) //if game has not started
        {
            if(Input.anyKeyDown) 
            {
                hasStarted = true; //start game if any key is pressed
            }
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f); //move arrows
        }
    }
}


//References: Used https://www.youtube.com/watch?v=cZzf1FQQFA0 as a reference for the code.