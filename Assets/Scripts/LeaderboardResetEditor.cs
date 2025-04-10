using UnityEngine;

public class LeaderboardResetEditor : MonoBehaviour
{
    public enum LeaderboardType { Easy, Medium, Hard }

    [SerializeField] private LeaderboardType leaderboardToReset;
    [SerializeField] private bool resetNow = false;

    void OnValidate()
    {
        if (resetNow)
        {
            switch (leaderboardToReset)
            {
                case LeaderboardType.Easy:
                    PlayerPrefs.DeleteKey("EasyHighscores");
                    Debug.Log(" Easy leaderboard reset.");
                    break;
                case LeaderboardType.Medium:
                    PlayerPrefs.DeleteKey("MediumHighscores");
                    Debug.Log(" Medium leaderboard reset.");
                    break;
                case LeaderboardType.Hard:
                    PlayerPrefs.DeleteKey("AllHighscores");
                    Debug.Log(" Hard leaderboard reset.");
                    break;
            }

            PlayerPrefs.Save();
            resetNow = false; // Uncheck automatically
        }
    }
}
