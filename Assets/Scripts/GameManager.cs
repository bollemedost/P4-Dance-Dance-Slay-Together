using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;
    public ArrowScrollerAioli theArrowScroller;

    public static GameManager instance; // Singleton pattern

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiplierText;

    public BeatLoader beatLoader;
    public ArrowSpawner arrowSpawner;
    private int nextBeatIndex = 0;
    private float delayTime = 5.5f; // Delay before music starts

    void Start()
    {
        instance = this;

        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currentMultiplier;

        if (beatLoader == null)
        {
            Debug.LogError("❌ BeatLoader is not assigned in GameManager!");
        }
        if (arrowSpawner == null)
        {
            Debug.LogError("❌ ArrowSpawner is not assigned in GameManager!");
        }
    }

    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theArrowScroller.hasStarted = true;

                // Start arrows immediately
                StartCoroutine(SpawnArrowsOnBeat());

                // Delay the music
                StartCoroutine(StartMusicWithDelay());
            }
        }
    }

    IEnumerator StartMusicWithDelay()
    {
        Debug.Log($"🎵 Waiting {delayTime} seconds before starting the music...");
        yield return new WaitForSeconds(delayTime);

        theMusic.Play();
    }

    IEnumerator SpawnArrowsOnBeat()
    {
        if (beatLoader == null || arrowSpawner == null)
        {
            Debug.LogError("❌ BeatLoader or ArrowSpawner is missing!");
            yield break;
        }

        List<float> beatTimes = beatLoader.GetBeatTimings();

        while (nextBeatIndex < beatTimes.Count)
        {
            float beatTime = beatTimes[nextBeatIndex] - GetTravelTime();

            while (Time.timeSinceLevelLoad < beatTime)
            {
                yield return null;
            }

            arrowSpawner.SpawnArrow();
            nextBeatIndex++;
        }
    }

    float GetTravelTime()
    {
        return 1.5f;
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        scoreText.text = "Score: " + currentScore;
        multiplierText.text = "Multiplier: x" + currentMultiplier;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
    }

    public void NoteMissed(GameObject missedArrow)
    {
        Debug.Log("❌ Missed Note - Resetting Multiplier");

        currentMultiplier = 1;
        multiplierTracker = 0;
        multiplierText.text = "Multiplier: x" + currentMultiplier;

        if (missedArrow != null)
        {
            Destroy(missedArrow);
        }
    }
}
