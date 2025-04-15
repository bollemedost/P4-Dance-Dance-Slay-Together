using System.Collections.Generic;
using UnityEngine;

public static class HighscoreManager
{
    private const string Key = "Highscores";
    private const int MaxScores = 10;

    public static List<int> GetHighscores()
    {
        string raw = PlayerPrefs.GetString(Key, "");
        List<int> scores = new List<int>();

        if (!string.IsNullOrEmpty(raw))
        {
            string[] parts = raw.Split(',');
            foreach (var p in parts)
            {
                if (int.TryParse(p, out int score))
                    scores.Add(score);
            }
        }

        return scores;
    }

    public static void SaveHighscore(int score)
    {
        List<int> scores = GetHighscores();
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // Descending
        if (scores.Count > MaxScores)
            scores.RemoveRange(MaxScores, scores.Count - MaxScores);

        PlayerPrefs.SetString(Key, string.Join(",", scores));
        PlayerPrefs.Save();
    }
}




//References: Used https://www.youtube.com/watch?v=EJIDPo-nW-Q as a reference for the code.
//References: Used https://www.youtube.com/watch?v=iAbaqGYdnyI as a reference for the code.
//References: Used Chatgpt as a reference for the code.