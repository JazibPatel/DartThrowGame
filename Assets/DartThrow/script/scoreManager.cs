<<<<<<< HEAD
﻿//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class scoreManager : MonoBehaviour
//{
//    public static scoreManager instance;

//    [Header("UI References")]
//    public TMP_Text redScoreText;
//    public TMP_Text blueScoreText;

//    [Header("Game Settings")]
//    public int maxDarts = 5;

//    [Header("Spawner Reference")]
//    public dartSpawner spawner;

//    [HideInInspector]
//    public int currentPlayer = 1; // 1 = Red, 2 = Blue

//    private int redScore = 0;
//    private int blueScore = 0;
//    private int redDartsThrown = 0;
//    private int blueDartsThrown = 0;
//    private bool gameOver = false;

//    public static int winnerPlayer = 0;

//    private bool isSoloMode;
//    private string difficulty;

//    void Awake()
//    {
//        instance = this;
//    }

//    void Start()
//    {
//        // Detect mode & difficulty from SceneLoader
//        if (SceneLoader.instance != null)
//        {
//            isSoloMode = (SceneLoader.instance.numOfPlayers == 1);
//            difficulty = SceneLoader.instance.difficulty;
//        }

//        Debug.Log($"Mode: {(isSoloMode ? "Solo" : "Duo")} | Difficulty: {difficulty}");

//        // Spawn first dart for Player 1 (Red)
//        spawner.SpawnDart(currentPlayer);
//    }

//    // Public getters for dartScript
//    public bool IsSoloMode => isSoloMode;
//    public string Difficulty => difficulty;

//    public void AddScore(int playerNumber, int points)
//    {
//        if (playerNumber == 1)
//        {
//            redScore += points;
//            redScoreText.text = redScore.ToString();
//        }
//        else
//        {
//            blueScore += points;
//            blueScoreText.text = blueScore.ToString();
//        }
//    }

//    public void RegisterThrow(int playerNumber)
//    {
//        if (playerNumber == 1)
//            redDartsThrown++;
//        else
//            blueDartsThrown++;

//        if (!gameOver && redDartsThrown >= maxDarts && blueDartsThrown >= maxDarts)
//            EndGame();
//    }

//    public void NextTurn()
//    {
//        if (gameOver)
//            return;

//        currentPlayer = (currentPlayer == 1) ? 2 : 1;
//        spawner.SpawnDart(currentPlayer);
//    }

//    private void EndGame()
//    {
//        gameOver = true;

//        if (redScore > blueScore)
//        {
//            Debug.Log("Game Over! Red Player Wins!");
//            winnerPlayer = 1;
//        }
//        else if (blueScore > redScore)
//        {
//            Debug.Log("Game Over! Blue Player Wins!");
//            winnerPlayer = 2;
//        }
//        else
//        {
//            Debug.Log("Game Over! It's a Tie!");
//            winnerPlayer = 0;
//        }

//        // 🔹 Optional: Add 2-second delay before loading Winner Scene
//        Invoke(nameof(LoadWinnerScene), 2f);
//    }

//    private void LoadWinnerScene()
//    {
//        SceneManager.LoadScene("WinnerScene");
//    }
//}


using TMPro;
=======
﻿using TMPro;
>>>>>>> 809d5065289469830f2b575a3eca940340a9340d
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

<<<<<<< HEAD
    private int[] easyArr = { 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 };
    private int[] mediumArr = { 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 };
    private int[] hardArr = { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 };
    public int WinOrLose;
=======
    // Arrays controlling bot win/lose bias
    private int[] easyArr = { 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 };
    private int[] mediumArr = { 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 };
    private int[] hardArr = { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 };
>>>>>>> 809d5065289469830f2b575a3eca940340a9340d

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

<<<<<<< HEAD
        if (isSoloMode)
        {
            int[] difficultyArr = new int[10];
            switch (difficulty.ToLower())
            {
                case "easy":
                    System.Array.Copy(easyArr, difficultyArr, easyArr.Length);
                    break;
                case "medium":
                    System.Array.Copy(mediumArr, difficultyArr, mediumArr.Length);
                    break;
                default: // hard
                    System.Array.Copy(hardArr, difficultyArr, hardArr.Length);
                    break;
            }
            WinOrLose = difficultyArr[UnityEngine.Random.Range(0, difficultyArr.Length)];
            Debug.Log($"Bot WinOrLose: {WinOrLose}"); // For testing
        }

=======
>>>>>>> 809d5065289469830f2b575a3eca940340a9340d
        // Spawn initial darts for both players
        spawner.SpawnDart(1);
        spawner.SpawnDart(2); // Always spawn blue dart, even in solo mode
    }

<<<<<<< HEAD
    // Public getters for dartScript
=======
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

>>>>>>> 809d5065289469830f2b575a3eca940340a9340d
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
<<<<<<< HEAD
            spawner.UpdateQueue(playerNumber); // Spawn next
=======
            spawner.UpdateQueue(playerNumber);
>>>>>>> 809d5065289469830f2b575a3eca940340a9340d
        }
        else
        {
            blueDartsThrown++;
<<<<<<< HEAD
            spawner.UpdateQueue(playerNumber); // Spawn next
=======
            spawner.UpdateQueue(playerNumber);
>>>>>>> 809d5065289469830f2b575a3eca940340a9340d
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
