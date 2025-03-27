﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;
    public static GameManager instance;

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
    public ArrowSpawner[] arrowSpawner;
    private int nextBeatIndex = 0;
    private float delayTime = 5.5f;

    public int normalHits = 0;
    public int goodHits = 0;
    public int perfectHits = 0;
    public int missedNotes = 0;

    private System.Random random;

    void Start()
    {
        instance = this;

        scoreText.text = "0";
        currentMultiplier = 1;
        multiplierText.text = "Multiplier: x" + currentMultiplier;

        if (beatLoader == null)
        {
            Debug.LogError("BeatLoader is not assigned in GameManager!");
        }
        if (arrowSpawner == null)
        {
            Debug.LogError("ArrowSpawner is not assigned in GameManager!");
        }

        random = new System.Random();
    }

    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                StartCoroutine(SpawnArrowsOnBeat());
                StartCoroutine(StartMusicWithDelay());
            }
        }
    }

    IEnumerator StartMusicWithDelay()
    {
        Debug.Log($"Waiting {delayTime} seconds before starting the music...");

        // 🧊 Pre-warm the audio system
        theMusic.volume = 0f;
        theMusic.Play();
        theMusic.Pause();

        yield return new WaitForSeconds(delayTime - 0.1f);

        theMusic.volume = 1f;
        theMusic.UnPause(); // Lag-free playback
    }

    IEnumerator SpawnArrowsOnBeat()
    {
        if (beatLoader == null || arrowSpawner == null)
        {
            Debug.LogError("BeatLoader or ArrowSpawner is missing!");
            yield break;
        }

        List<float> beatTimes = beatLoader.GetBeatTimings();

        while (nextBeatIndex < beatTimes.Count)
        {
            float beatTime = beatTimes[nextBeatIndex] - GetTravelTime();

            // ⛔ Skip if the beat has already passed
            if (Time.timeSinceLevelLoad > beatTime)
            {
                nextBeatIndex++;
                continue;
            }

            // ✅ Wait until the right time
            while (Time.timeSinceLevelLoad < beatTime)
            {
                yield return null;
            }

            int arrowType = random.Next(0, 4);
            foreach (ArrowSpawner spawner in arrowSpawner)
            {
                spawner.SpawnArrow(arrowType);
            }

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

        scoreText.text = currentScore.ToString();
        multiplierText.text = "Multiplier: x" + currentMultiplier;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        normalHits++;
        Debug.Log($"Normal Hit Count: {normalHits}");
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        goodHits++;
        Debug.Log($"Good Hit Count: {goodHits}");
        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        perfectHits++;
        Debug.Log($"Perfect Hit Count: {perfectHits}");
        NoteHit();
    }

    public void NoteMissed(GameObject missedArrow)
    {
        Debug.Log("Missed Note - Resetting Multiplier");

        currentMultiplier = 1;
        multiplierTracker = 0;
        multiplierText.text = "Multiplier: x" + currentMultiplier;

        missedNotes++;
        Debug.Log($"Missed Note Count: {missedNotes}");

        if (missedArrow != null)
        {
            Destroy(missedArrow);
        }
    }
}
