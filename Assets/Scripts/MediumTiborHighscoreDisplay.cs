using UnityEngine;
using TMPro;

public class MediumTiborHighscoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreListText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        string lastName = PlayerPrefs.GetString("LastTeamName", "YOU");

        scoreText.text = "Your Score: " + lastScore;

        var list = MediumTiborHighscoreManager.LoadHighscores();

        string formatted = "Top Highscores:\n";
        for (int i = 0; i < list.entries.Count; i++)
        {
            var entry = list.entries[i];
            formatted += $"{i + 1}. {entry.name} - {entry.score}\n";
        }

        highscoreListText.text = formatted;
    }
}
