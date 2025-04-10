using UnityEngine;
using TMPro;

public class TiborHighscoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreListText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        string lastName = PlayerPrefs.GetString("LastTeamName", "YOU");

        scoreText.text = "Your Score: " + lastScore;

        HighscoreList highscores = TiborHighscoreManager.LoadHighscoreList();

        string formatted = "Top Highscores:\n";
        for (int i = 0; i < highscores.entries.Count; i++)
        {
            var entry = highscores.entries[i];
            formatted += $"{i + 1}. {entry.name} - {entry.score}\n";
        }

        highscoreListText.text = formatted;
    }
}
