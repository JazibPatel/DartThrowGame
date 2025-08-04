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

    void Awake() => instance = this;

    void Start()
    {
        Debug.Log("Spawning first dart for Player 1");
        spawner.SpawnDart(currentPlayer);
    }

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
            SceneManager.LoadScene("WinnerScene");

        }
        else if (blueScore > redScore)
        {
            Debug.Log("Game Over! Blue Player Wins!");
            winnerPlayer = 2;
            SceneManager.LoadScene("WinnerScene");
        }
        else
        {
            Debug.Log("Game Over! It's a Tie!");
            winnerPlayer = 0;
            SceneManager.LoadScene("WinnerScene");
        }
    }
}
