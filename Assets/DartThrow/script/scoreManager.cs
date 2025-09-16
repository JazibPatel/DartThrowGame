using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scoreManager : MonoBehaviour
{
    public static scoreManager instance;

    [Header("UI References")]
    public TMP_Text redScoreText;
    public TMP_Text blueScoreText;

    [Header("Game Settings")]
    public int maxDarts = 5;

    [Header("Spawner Reference")]
    public dartSpawner spawner;

    private int redScore = 0;
    private int blueScore = 0;
    private int redDartsThrown = 0;
    private int blueDartsThrown = 0;
    private bool gameOver = false;

    public static int winnerPlayer = 0;

    private bool isSoloMode;
    private string difficulty;

    // Arrays controlling bot win/lose bias
    private int[] easyArr = { 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 };
    private int[] mediumArr = { 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 };
    private int[] hardArr = { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 };

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Detect mode & difficulty from SceneLoader
        if (SceneLoader.instance != null)
        {
            isSoloMode = (SceneLoader.instance.numOfPlayers == 1);
            difficulty = SceneLoader.instance.difficulty;
        }

        Debug.Log($"Mode: {(isSoloMode ? "Solo" : "Duo")} | Difficulty: {difficulty}");

        // Spawn initial darts for both players
        spawner.SpawnDart(1);
        spawner.SpawnDart(2); // Always spawn blue dart, even in solo mode
    }

    // Called by dartScript to decide bot's goal for *this turn*
    public int GetWinOrLose()
    {
        if (!isSoloMode) return 0; // No bot logic in multiplayer

        int[] difficultyArr;
        switch (difficulty.ToLower())
        {
            case "easy":
                difficultyArr = easyArr;
                break;
            case "medium":
                difficultyArr = mediumArr;
                break;
            default:
                difficultyArr = hardArr;
                break;
        }

        int result = difficultyArr[Random.Range(0, difficultyArr.Length)];
        Debug.Log($"[scoreManager] Bot WinOrLose this turn = {result}");
        return result; // 1 = bot aims to win, 0 = bot aims to lose
    }

    public bool IsSoloMode => isSoloMode;
    public string Difficulty => difficulty;

    public void AddScore(int playerNumber, int points)
    {
        if (playerNumber == 1)
        {
            redScore += points;
            redScoreText.text = redScore.ToString();
            Debug.Log("Red Score = " + redScore);
        }
        else
        {
            blueScore += points;
            blueScoreText.text = blueScore.ToString();
            Debug.Log("Blue Score = " + blueScore);
        }
    }

    public void RegisterCompletedThrow(int playerNumber)
    {
        if (playerNumber == 1)
        {
            redDartsThrown++;
            spawner.UpdateQueue(playerNumber);
        }
        else
        {
            blueDartsThrown++;
            spawner.UpdateQueue(playerNumber);
        }

        if (!gameOver && redDartsThrown >= maxDarts && blueDartsThrown >= maxDarts)
            EndGame();
    }

    private void EndGame()
    {
        gameOver = true;

        if (redScore > blueScore)
        {
            Debug.Log("Game Over! Red Player Wins!");
            winnerPlayer = 1;
        }
        else if (blueScore > redScore)
        {
            Debug.Log("Game Over! Blue Player Wins!");
            winnerPlayer = 2;
        }
        else
        {
            Debug.Log("Game Over! It's a Tie!");
            winnerPlayer = 0;
        }

        Invoke(nameof(LoadWinnerScene), 2f);
    }

    private void LoadWinnerScene()
    {
        SceneManager.LoadScene("WinnerScene");
    }
}
