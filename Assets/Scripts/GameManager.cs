using System.Collections;
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
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                gameStartTime = Time.timeSinceLevelLoad;
                StartCoroutine(SpawnArrowsOnBeat());
                StartCoroutine(StartMusicWithDelay());
            }
        }

        // If music has finished playing
        if (startPlaying && !theMusic.isPlaying && theMusic.time > 1f)
        {
            HighscoreManager.SaveHighscore(currentScore);
            UnityEngine.SceneManagement.SceneManager.LoadScene("HighScore");
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
                Debug.Log($"⏩ Skipped beat at {beatTime}s (too early)");
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

        if (multiplierIncreased && multiplierIncreaseParticles != null)
        {
            PlayRandomParticle(multiplierIncreaseParticles);
            SoundManager.Instance.PlaySwooshBling();
            SoundManager.Instance.PlayKeepSlaying();
        }

        scoreText.text = currentScore.ToString();
        multiplierText.text = "x" + currentMultiplier;

        UpdateSliderAndStars();
    }


    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        normalHits++;
        Debug.Log($"Normal Hit Count: {normalHits}");

        if (normalHitParticles != null)
        PlayRandomParticle(normalHitParticles);
        
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        goodHits++;
        Debug.Log($"Good Hit Count: {goodHits}");

        if (goodHitParticles != null)
        PlayRandomParticle(goodHitParticles);

        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        perfectHits++;
        Debug.Log($"Perfect Hit Count: {perfectHits}");

        if (perfectHitParticles != null)
        PlayRandomParticle(perfectHitParticles);

        NoteHit();
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
}
