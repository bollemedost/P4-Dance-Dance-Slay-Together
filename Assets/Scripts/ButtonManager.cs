using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject difficultyPanel; // Assign DifficultyPanel in Inspector

    public string easySceneName = "Aioli";
    public string mediumSceneName = "Aioli";
    public string hardSceneName = "Aioli";

    public string backToMainMenu = "Main Menu";

    public void Start()
    {
        difficultyPanel.SetActive(false); // Hide DifficultyPanel on Start
    }

    public void OnPlayButtonClick()
    {
        difficultyPanel.SetActive(true); // Show DifficultyPanel when Play button is clicked
    }

    public void OnEasyMode()
    {
        SceneManager.LoadScene(easySceneName); // Load EasyScene when Easy button is clicked
    }

    public void OnMediumMode()
    {
        SceneManager.LoadScene(mediumSceneName); // Load EasyScene when Easy button is clicked
    }

    public void OnHardMode()
    {
        SceneManager.LoadScene(hardSceneName); // Load EasyScene when Easy button is clicked
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene(backToMainMenu); // Load EasyScene when Easy button is clicked
    }
}
