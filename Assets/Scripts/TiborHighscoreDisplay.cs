using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TiborHighscoreDisplay : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreListText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        string lastName = PlayerPrefs.GetString("LastTeamName", "YOU");

        scoreText.text = "" + lastScore;

        HighscoreList highscores = TiborHighscoreManager.LoadHighscoreList();

        string formatted = "\n";
        for (int i = 0; i < highscores.entries.Count; i++)
        {
            var entry = highscores.entries[i];
            formatted += $"{i + 1}. {entry.name} - {entry.score}\n";
        }

        highscoreListText.text = formatted;
    }
}
