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
    private float delayTime = 5.524f;

    public int normalHits = 0;
    public int goodHits = 0;
    public int perfectHits = 0;
    public int missedNotes = 0;

    private System.Random random;

    public Slider pointSlider;
    public Image[] starImages;
    public int[] starThresholds;
    public GameObject[] starOutlines;

    private bool[] starActivated;

    private float gameStartTime;

    public ParticleSystem[] perfectHitParticles;
    public ParticleSystem[] goodHitParticles;
    public ParticleSystem[] normalHitParticles;
    public ParticleSystem[] multiplierIncreaseParticles;
    public ParticleSystem[] starRewardParticles;

    private int totalHits = 0;
    public int normalHitInterval = 8; // Set how often the normal hit particle plays
    public int goodHitInterval = 8;   // Set how often the good hit particle plays
    public int perfectHitInterval = 8; // Set how often the perfect hit particle plays
    public int multiplierHitInterval = 2; // Set how often the multiplier particle plays


    void Start()
    {
        instance = this;

        scoreText.text = "0";
        currentMultiplier = 1;
        multiplierText.text = "x" + currentMultiplier;

        if (beatLoader == null)
        {
            Debug.LogError("BeatLoader is not assigned in GameManager!");
        }
        if (arrowSpawner == null)
        {
            Debug.LogError("ArrowSpawner is not assigned in GameManager!");
        }

        random = new System.Random();

        pointSlider.minValue = 0;
        pointSlider.maxValue = starThresholds[starThresholds.Length - 1];
        pointSlider.value = 0;

        for (int i = 0; i < starOutlines.Length; i++)
        {
            Transform fillStar = starOutlines[i].transform.Find($"Star{i + 1}Fill");
            if (fillStar != null)
            {
                fillStar.gameObject.SetActive(false);
            }
        }
        starActivated = new bool[starOutlines.Length];
    }

    void Update()
    {

        // If music has finished playing
        if (startPlaying && !theMusic.isPlaying && theMusic.time > 1f)
        {
            string lastName = PlayerPrefs.GetString("LastTeamName", "YOU");
            PlayerPrefs.SetInt("LastScore", currentScore);

            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentScene == "EasyAioli")
            {
                EasyTiborHighscoreManager.SaveHighscore(lastName, currentScore);
                UnityEngine.SceneManagement.SceneManager.LoadScene("EasyTiborHighscore");
            }
            else if (currentScene == "MediumAioli")
            {
                MediumTiborHighscoreManager.SaveHighscore(lastName, currentScore);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MediumTiborHighscore");
            }
            else
            {
                TiborHighscoreManager.SaveHighscore(lastName, currentScore);
                UnityEngine.SceneManagement.SceneManager.LoadScene("TiborHighscore");
            }
        }



    }

    IEnumerator StartMusicWithDelay()
    {
        Debug.Log($"Waiting {delayTime} seconds before starting the music...");

        // Pre-warm audio system
        theMusic.volume = 0f;
        theMusic.Play();
        theMusic.Pause();

        yield return new WaitForSeconds(delayTime - 0.1f);

        theMusic.volume = 1f;
        theMusic.UnPause(); // Sample-perfect resume
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
            float beatTime = beatTimes[nextBeatIndex];
            float spawnTime = beatTime - GetTravelTime();

            // Skip spawning if arrow would appear too early (e.g., within first second of game)
            if (spawnTime < 1.0f)
            {
                Debug.Log($" Skipped beat at {beatTime}s (too early)");
                nextBeatIndex++;
                continue;
            }

            while (Time.timeSinceLevelLoad - gameStartTime < spawnTime)
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

        bool multiplierIncreased = false;

        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
                multiplierIncreased = true;
            }
        }

        // Only play the multiplier particle when multiplier increases and the interval is met
        if (multiplierIncreased && multiplierIncreaseParticles != null && multiplierIncreaseParticles.Length > 0)
        {
            if (totalHits % multiplierHitInterval == 0)
            {
                PlayRandomParticle(multiplierIncreaseParticles);
                SoundManager.Instance.PlayRandomMotivationalSound();
            }
        }

        scoreText.text = currentScore.ToString();
        multiplierText.text = "x" + currentMultiplier;

        UpdateSliderAndStars();
    }


    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        normalHits++;
        totalHits++; // Increment the total hit counter
        Debug.Log($"Normal Hit Count: {normalHits}");

        // Check if it's the set interval (e.g., every 3rd hit)
        if (totalHits % normalHitInterval == 0)
        {
            // Play the normal hit particle system only for the set interval
            if (normalHitParticles != null && normalHitParticles.Length > 0)
            {
                PlayRandomParticle(normalHitParticles);
            }
        }

        NoteHit(); // Call NoteHit() to handle multiplier and score
    }


  public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        goodHits++;
        Debug.Log($"Good Hit Count: {goodHits}");

        // Check if it's the set interval for good hits
        if (totalHits % goodHitInterval == 0)
        {
            // Play the good hit particle system only for the set interval
            if (goodHitParticles != null && goodHitParticles.Length > 0)
            {
                PlayRandomParticle(goodHitParticles);
            }
        }

        NoteHit(); // Call NoteHit() to handle multiplier and score
    }


  public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        perfectHits++;
        totalHits++; // Increment the total hit counter for all hits
        Debug.Log($"Perfect Hit Count: {perfectHits}");

        // Check if it's the set interval (e.g., every 3rd hit)
        if (totalHits % perfectHitInterval == 0)
        {
            // Play the perfect hit particle system only for the set interval
            if (perfectHitParticles != null && perfectHitParticles.Length > 0)
            {
                PlayRandomParticle(perfectHitParticles);
            }
        }

        NoteHit(); // Call NoteHit() to handle multiplier and score
    }


    public void NoteMissed(GameObject missedArrow)
    {
        Debug.Log("Missed Note - Resetting Multiplier");

        currentMultiplier = 1;
        multiplierTracker = 0;
        multiplierText.text = "x" + currentMultiplier;

        missedNotes++;
        Debug.Log($"Missed Note Count: {missedNotes}");

        if (missedArrow != null)
        {
            Destroy(missedArrow);
        }
    }

  void UpdateSliderAndStars()
    {
        pointSlider.value = currentScore;

        for (int i = 0; i < starOutlines.Length; i++)
        {
            Transform fillStar = starOutlines[i].transform.Find($"Star{i + 1}Fill");
            if (fillStar != null)
            {
                bool shouldBeActive = pointSlider.value >= starThresholds[i];
                fillStar.gameObject.SetActive(shouldBeActive);

                if (shouldBeActive && !starActivated[i])
                {
                    StarPopAnimation pop = fillStar.GetComponent<StarPopAnimation>();
                    if (pop != null)
                    {
                        pop.PlayPop();
                    }

                    // Play random star reward particle
                    if (starRewardParticles != null && starRewardParticles.Length > 0)
                    {
                        int rand = Random.Range(0, starRewardParticles.Length);
                        ParticleSystem ps = starRewardParticles[rand];
                        if (ps != null)
                        {
                            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                            ps.Play();
                            SoundManager.Instance.PlayStarFairy();
                            Debug.Log($"⭐ Played star reward particle: {ps.name}");
                        }
                    }

                    starActivated[i] = true;
                }
            }
        }
    }


    void PlayRandomParticle(ParticleSystem[] particles)
    {
        if (particles != null && particles.Length > 0)
        {
            int index = Random.Range(0, particles.Length);
            if (particles[index] != null)
                particles[index].Play();
        }
    }
    public void SetGameStartTime()
    {
        gameStartTime = Time.timeSinceLevelLoad;
    }

    public void BeginGameplay()
    {
        StartCoroutine(SpawnArrowsOnBeat());
        StartCoroutine(StartMusicWithDelay());
    }
}



//chatgpt