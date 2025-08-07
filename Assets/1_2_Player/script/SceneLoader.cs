using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [Header("Game Mode Data")]
    public int numOfPlayers;       // 1 = Solo, 2 = Duo
    public string difficulty;      // "Easy", "Medium", "Hard" for Solo mode

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy old duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persistent
    }

    // Called when user selects SOLO mode
    public void SelectSoloMode()
    {
        numOfPlayers = 1;
        difficulty = ""; // Will be set after stage selection
    }

    // Called when user selects DUO mode
    public void SelectDuoMode()
    {
        numOfPlayers = 2;
        difficulty = ""; // Duo mode has no difficulty
        LoadGameList();
    }

    // Called when user selects Solo Stage (Easy/Medium/Hard)
    public void SelectSoloDifficulty(string diff)
    {
        difficulty = diff;
        LoadGameList();  // Load same game list for solo mode
    }

    // Load Game List Scene (same for Solo & Duo)
    public void LoadGameList()
    {
        SceneManager.LoadScene("gameList");
    }

    // Used by game list buttons to load a game (like Dart Throw)
    public void LoadGame(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ResetData()
    {
        numOfPlayers = 0;
        difficulty = "";
    }

    public void BackToMain()
    {
        ResetData(); // Clear old values

        Destroy(gameObject); // Destroy SceneLoader instance
        SceneManager.LoadScene("Main"); // Load Main scene fresh
    }
}

