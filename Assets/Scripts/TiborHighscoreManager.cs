using System.Collections.Generic;
using UnityEngine;

public static class TiborHighscoreManager
{
    private const string HighscoresKey = "AllHighscores";
    private const int MaxHighscores = 10;

    public static void SaveHighscore(int score)
    {
        List<int> highscores = LoadHighscores();
        highscores.Add(score);
        highscores.Sort((a, b) => b.CompareTo(a)); // Highest first

        if (highscores.Count > MaxHighscores)
            highscores.RemoveAt(highscores.Count - 1); // Keep top 10

        string saveData = string.Join(",", highscores);
        PlayerPrefs.SetString(HighscoresKey, saveData);
        PlayerPrefs.Save();
    }

    public static List<int> LoadHighscores()
    {
        string data = PlayerPrefs.GetString(HighscoresKey, "");
        List<int> highscores = new List<int>();

        if (!string.IsNullOrEmpty(data))
        {
            string[] parts = data.Split(',');
            foreach (string part in parts)
            {
                if (int.TryParse(part, out int score))
                    highscores.Add(score);
            }
        }

        return highscores;
    }
}
