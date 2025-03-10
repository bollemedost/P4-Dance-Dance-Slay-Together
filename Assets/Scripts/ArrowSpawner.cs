using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject[] arrowPrefabs; // Assign 4 arrow prefabs (Left, Down, Up, Right)
    public Transform[] spawnPoints; // Assign 8 spawn points (one for each arrow type for each player)
    public TextAsset beatDataFile; // Drag & Drop Unwritten_beat_timings.json into Inspector
    public AudioSource music; // Assign MusicPlayer's AudioSource in Inspector

    private List<float> beatTimes = new List<float>();
    private bool hasStarted = false; // Prevents duplicate spawning

    void Start()
    {
        LoadBeats();
    }

    public void StartSpawning()
    {
        if (hasStarted) return; // Prevents spawning twice

        if (!music.isPlaying)
        {
            Debug.LogError("Music is not playing! Make sure GameManager starts the song.");
            return;
        }

        hasStarted = true; // Ensures arrows only spawn once
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

            SpawnArrow();
        }
    }

    public void SpawnArrow()
    {
        if (arrowPrefabs.Length != 4 || spawnPoints.Length != 4)
        {
            Debug.LogError("Ensure there are exactly 4 arrow prefabs and 4 spawn points assigned.");
            return;
        }

        int arrowType = Random.Range(0, 4); // 0 = Left, 1 = Down, 2 = Up, 3 = Right
        //int player = Random.Range(0, 2); // 0 = Player 1, 1 = Player 2

        //int spawnIndex = arrowType + (player * 4); // Offset for Player 2's spawn points
        //Transform spawnPoint = spawnPoints[spawnIndex]; // Get correct spawn position

        Transform spawnPoint = spawnPoints[arrowType]; // Get correct spawn position

        GameObject newArrow = Instantiate(arrowPrefabs[arrowType], spawnPoint.position, Quaternion.identity);

        // Make sure it starts falling
        FallingArrow fallingArrow = newArrow.GetComponent<FallingArrow>();
        if (fallingArrow != null)
        {
            fallingArrow.StartFalling();
        }

        Debug.Log($"Spawned {arrowPrefabs[arrowType].name} at {spawnPoint.position}");
    }

    float GetTravelTime()
    {
        return 1.5f; // Adjust based on how long arrows should take to reach the bottom
    }

    [System.Serializable]
    private class BeatData
    {
        public float[] beats;
    }
}
