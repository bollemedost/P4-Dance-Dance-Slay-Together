using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EasyHighscoreEntry { public string name; public int score; }

[System.Serializable]
public class EasyHighscoreList { public List<EasyHighscoreEntry> entries = new(); }

public static class EasyTiborHighscoreManager
{
    private const string Key = "EasyHighscores";
    private const int Max = 10;

    public static void SaveHighscore(string name, int score)
    {
        EasyHighscoreList list = LoadHighscores();
        list.entries.Add(new EasyHighscoreEntry { name = name, score = score });
        list.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (list.entries.Count > Max) list.entries.RemoveRange(Max, list.entries.Count - Max);

        PlayerPrefs.SetString(Key, JsonUtility.ToJson(list));
        PlayerPrefs.Save();
    }

    public static EasyHighscoreList LoadHighscores()
    {
        string json = PlayerPrefs.GetString(Key, "");
        if (!string.IsNullOrEmpty(json))
        {
            try { return JsonUtility.FromJson<EasyHighscoreList>(json); }
            catch { PlayerPrefs.DeleteKey(Key); PlayerPrefs.Save(); }
        }
        return new EasyHighscoreList();
    }
}
