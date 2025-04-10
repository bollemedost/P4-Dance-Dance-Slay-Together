using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MediumHighscoreEntry { public string name; public int score; }

[System.Serializable]
public class MediumHighscoreList { public List<MediumHighscoreEntry> entries = new(); }

public static class MediumTiborHighscoreManager
{
    private const string Key = "MediumHighscores";
    private const int Max = 10;

    public static void SaveHighscore(string name, int score)
    {
        MediumHighscoreList list = LoadHighscores();
        list.entries.Add(new MediumHighscoreEntry { name = name, score = score });
        list.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (list.entries.Count > Max) list.entries.RemoveRange(Max, list.entries.Count - Max);

        PlayerPrefs.SetString(Key, JsonUtility.ToJson(list));
        PlayerPrefs.Save();
    }

    public static MediumHighscoreList LoadHighscores()
    {
        string json = PlayerPrefs.GetString(Key, "");
        if (!string.IsNullOrEmpty(json))
        {
            try { return JsonUtility.FromJson<MediumHighscoreList>(json); }
            catch { PlayerPrefs.DeleteKey(Key); PlayerPrefs.Save(); }
        }
        return new MediumHighscoreList();
    }
}
