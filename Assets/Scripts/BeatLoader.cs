using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BeatLoader : MonoBehaviour
{
    public TextAsset beatDataFile;
    private List<float> beatTimings;

    void Start()
    {
        LoadBeats();
    }

    void LoadBeats()
    {
        if (beatDataFile != null)
        {
            BeatData data = JsonUtility.FromJson<BeatData>(beatDataFile.text);
            beatTimings = new List<float>(data.beats);
            Debug.Log("Loaded " + beatTimings.Count + " beats.");
        }
        else
        {
            Debug.LogError("❌ Beat file not assigned!");
        }
    }

    // 🔹 New Method: Allows GameManager to access beat timings
    public List<float> GetBeatTimings()
    {
        return beatTimings;
    }
}

[System.Serializable]
public class BeatData
{
    public List<float> beats;
}
