using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;
    public ArrowScrollerAioli theArrowScroller; 

    public static GameManager instance; //reference to GameManager; static because there should only be one GameManager

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiplierText;

    void Start()
    {
        instance = this; //set instance to this GameManager

        scoreText.text = "Score: 0"; //sets score to 0 at beginning of game
        currentMultiplier = 1; //set multiplier to 1 at beginning of game
        multiplierText.text = "Multiplier: x" + currentMultiplier; //updates current multiplier
    }
   
    void Update()
    {
        if(!startPlaying)
        {
            if(Input.anyKeyDown)
            {
                startPlaying = true;
                theArrowScroller.hasStarted = true;
                theMusic.Play();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        if(currentMultiplier - 1 < multiplierThresholds.Length) //if the current multiplier is less than the length of the multiplier thresholds
        {
            multiplierTracker++; //add 1 to multiplier tracker

            if(multiplierThresholds[currentMultiplier - 1] <= multiplierTracker) //if the current multiplier is less than or equal to the multiplier tracker
            {
                multiplierTracker = 0; //reset multiplier tracker
                currentMultiplier++; //add 1 to current multiplier
            }
        }

        scoreText.text = "Score: " + currentScore; //updates current score
        
        multiplierText.text = "Multiplier: x" + currentMultiplier; //updates multiplier text
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier; //add score to current multiplier
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier; //add score to current multiplier
        NoteHit();
    } 

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier; //add score to current multiplier
        NoteHit(); 
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");

        currentMultiplier = 1; //reset multiplier to 1
        multiplierTracker = 0; //reset multiplier tracker
        multiplierText.text = "Multiplier: x" + currentMultiplier; //updates multiplier text
    }

}

// References: Used https://www.youtube.com/@gamesplusjames as a reference for the code. 
