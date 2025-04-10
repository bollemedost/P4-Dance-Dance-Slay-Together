using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class HighscoreList
{
    public List<HighscoreEntry> entries = new List<HighscoreEntry>();
}

public static class TiborHighscoreManager
{
    private const string HighscoresKey = "AllHighscores";
    private const int MaxHighscores = 10;

    public static void SaveHighscore(string name, int score)
    {
        HighscoreList list = LoadHighscoreList();

        list.entries.Add(new HighscoreEntry { name = name, score = score });
        list.entries.Sort((a, b) => b.score.CompareTo(a.score)); // Highest first

        if (list.entries.Count > MaxHighscores)
            list.entries.RemoveRange(MaxHighscores, list.entries.Count - MaxHighscores);

        string json = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(HighscoresKey, json);
        PlayerPrefs.Save();
    }

    public static HighscoreList LoadHighscoreList()
    {
        string json = PlayerPrefs.GetString(HighscoresKey, "");

        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                return JsonUtility.FromJson<HighscoreList>(json);
            }
            catch
            {
                Debug.LogWarning("Failed to parse highscores JSON, resetting...");
                PlayerPrefs.DeleteKey(HighscoresKey);
                PlayerPrefs.Save();
            }
        }

        return new HighscoreList();
    }
}
