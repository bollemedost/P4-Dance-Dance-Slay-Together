using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MediumTiborHighscoreDisplay : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreListText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        string lastName = PlayerPrefs.GetString("LastTeamName", "YOU");

        scoreText.text = "" + lastScore;

        var list = MediumTiborHighscoreManager.LoadHighscores();

        string formatted = "\n";
        for (int i = 0; i < list.entries.Count; i++)
        {
            var entry = list.entries[i];
            formatted += $"{i + 1}. {entry.name} - {entry.score}\n";
        }

        highscoreListText.text = formatted;
    }



}



//References: Used https://www.youtube.com/watch?v=EJIDPo-nW-Q as a reference for the code.
//References: Used https://www.youtube.com/watch?v=iAbaqGYdnyI as a reference for the code.
//References: Used Chatgpt as a reference for the code.
