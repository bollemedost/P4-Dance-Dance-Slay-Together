using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{
    public Transform highscoreEntryContainer;
    public Transform highscoreEntryTemplate;

    void Start()
    {
        highscoreEntryTemplate.gameObject.SetActive(false); // Hide template

        List<int> scores = HighscoreManager.GetHighscores();
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        // Show player's score at the top
        AddEntryToTop("YOU", lastScore, 1);

        // Show rest of the highscore list
        for (int i = 0; i < scores.Count; i++)
        {
            string name = "AAA"; // Placeholder, you can later expand to allow player name input
            AddEntry(name, scores[i], i + 1);
        }
    }

    void AddEntry(string name, int score, int position)
    {
        Transform entry = Instantiate(highscoreEntryTemplate, highscoreEntryContainer);
        entry.gameObject.SetActive(true);

        entry.Find("Position").GetComponent<Text>().text = position.ToString();
        entry.Find("Name").GetComponent<Text>().text = name;
        entry.Find("Score").GetComponent<Text>().text = score.ToString();
    }

    void AddEntryToTop(string name, int score, int position)
    {
        Transform entry = Instantiate(highscoreEntryTemplate, highscoreEntryContainer);
        entry.SetSiblingIndex(0); // Put on top
        entry.gameObject.SetActive(true);

        entry.Find("Position").GetComponent<Text>().text = position.ToString();
        entry.Find("Name").GetComponent<Text>().text = name;
        entry.Find("Score").GetComponent<Text>().text = score.ToString();
    }
}



//References: Used https://www.youtube.com/watch?v=EJIDPo-nW-Q as a reference for the code.
//References: Used https://www.youtube.com/watch?v=iAbaqGYdnyI as a reference for the code.
//References: Used Chatgpt as a reference for the code.