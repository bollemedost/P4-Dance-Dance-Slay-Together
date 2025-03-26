using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // Assign 4 arrow prefabs (Left, Down, Up, Right)
    public Transform[] spawnPoints; // Assign 4 spawn points (one for each arrow type for each player)
    public TextAsset beatDataFile;
    public AudioSource music;

    private List<float> beatTimes = new List<float>();
    private bool hasStarted = false;

    public int playerNumber = 1; // Set to 1 for Player 1, 2 for Player 2 in Inspector

    void Start()
    {
        LoadBeats();
    }

    public void StartSpawning()
    {
        if (hasStarted) return;

        if (!music.isPlaying)
        {
            Debug.LogError("Music is not playing! Make sure GameManager starts the song.");
            return;
        }

        hasStarted = true;
        StartCoroutine(SpawnArrows());
    }

    void LoadBeats()
    {
        if (beatDataFile != null)
        {
            BeatData data = JsonUtility.FromJson<BeatData>(beatDataFile.text);
            beatTimes = new List<float>(data.beats);
            Debug.Log($"Loaded {beatTimes.Count} beats from JSON.");
        }
        else
        {
            Debug.LogError("Beat file not assigned!");
        }
    }

    IEnumerator SpawnArrows()
    {
        if (arrowPrefabs.Length != 4 || spawnPoints.Length != 4)
        {
            Debug.LogError("Ensure there are exactly 4 arrow prefabs and 4 spawn points assigned.");
            yield break;
        }

        foreach (float beatTime in beatTimes)
        {
            yield return new WaitUntil(() => music.time >= beatTime - GetTravelTime());
            int arrowType = Random.Range(0, 4);
            SpawnArrow(arrowType);
        }
    }

    public void SpawnArrow(int arrowType)
    {
        if (arrowPrefabs.Length != 4 || spawnPoints.Length != 4)
        {
            Debug.LogError("Ensure there are exactly 4 arrow prefabs and 4 spawn points assigned.");
            return;
        }

        Transform spawnPoint = spawnPoints[arrowType];
        GameObject newArrow = Instantiate(arrowPrefabs[arrowType], spawnPoint.position, Quaternion.identity);

        ArrowDetectorAioli detector = newArrow.GetComponent<ArrowDetectorAioli>();
        if (detector != null)
        {
            detector.playerNumber = playerNumber;
        }

        FallingArrow fallingArrow = newArrow.GetComponent<FallingArrow>();
        if (fallingArrow != null)
        {
            fallingArrow.StartFalling();
        }

        Debug.Log($"Spawned {arrowPrefabs[arrowType].name} for Player {playerNumber} at {spawnPoint.position}");
    }

    float GetTravelTime()
    {
        return 1.5f;
    }

    [System.Serializable]
    private class BeatData
    {
        public float[] beats;
    }
}
