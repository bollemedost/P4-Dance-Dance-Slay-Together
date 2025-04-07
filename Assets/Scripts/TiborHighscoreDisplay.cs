using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TiborHighscoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreListText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        scoreText.text = "Your Score: " + lastScore;

        List<int> highscores = TiborHighscoreManager.LoadHighscores();

        string formatted = "Top Highscores:\n";
        for (int i = 0; i < highscores.Count; i++)
        {
            formatted += $"{i + 1}. {highscores[i]}\n";
        }

        highscoreListText.text = formatted;
    }
}
