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

    [HideInInspector]
    public int currentPlayer = 1; // 1 = Red, 2 = Blue

    private int redScore = 0;
    private int blueScore = 0;
    private int redDartsThrown = 0;
    private int blueDartsThrown = 0;
    private bool gameOver = false;

    public static int winnerPlayer = 0;

    private bool isSoloMode;
    private string difficulty;

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

        // Spawn first dart for Player 1 (Red)
        spawner.SpawnDart(currentPlayer);
    }

    // Public getters for dartScript
    public bool IsSoloMode => isSoloMode;
    public string Difficulty => difficulty;

    public void AddScore(int playerNumber, int points)
    {
        if (playerNumber == 1)
        {
            redScore += points;
            redScoreText.text = redScore.ToString();
        }
        else
        {
            blueScore += points;
            blueScoreText.text = blueScore.ToString();
        }
    }

    public void RegisterThrow(int playerNumber)
    {
        if (playerNumber == 1)
            redDartsThrown++;
        else
            blueDartsThrown++;

        if (!gameOver && redDartsThrown >= maxDarts && blueDartsThrown >= maxDarts)
            EndGame();
    }

    public void NextTurn()
    {
        if (gameOver)
            return;

        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        spawner.SpawnDart(currentPlayer);
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

        // 🔹 Optional: Add 2-second delay before loading Winner Scene
        Invoke(nameof(LoadWinnerScene), 2f);
    }

    private void LoadWinnerScene()
    {
        SceneManager.LoadScene("WinnerScene");
    }
}
