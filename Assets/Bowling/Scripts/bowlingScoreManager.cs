using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class bowlingScoreManager : MonoBehaviour
{
    public static bowlingScoreManager Instance;

    private int player1Score = 0;
    private int player2Score = 0;

    public TextMeshProUGUI redScore;
    public TextMeshProUGUI blueScore;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int player, int points)
    {
        if (player == 1)
        {
            player1Score += points;
            redScore.text = player1Score.ToString();
        }
        else
        {
            player2Score += points;
            blueScore.text = player2Score.ToString();
        }
        //Debug.Log($"Player 1: {player1Score} | Player 2: {player2Score}");
    }

    public void checkWinner()
    {
        if (gameManager.Instance.cameraFlipCount == 10)
        {
            if (player1Score > player2Score)
            {
                PlayerPrefs.SetString("winner", "Red");
                SceneManager.LoadScene("BowlingWinnerScene");
                Debug.Log("Red Player Win!!!");
            }
            else if (player1Score < player2Score)
            {
                PlayerPrefs.SetString("winner", "Blue");
                SceneManager.LoadScene("BowlingWinnerScene");
                Debug.Log("Blue Player Win!!!");
            }
            else
            {
                PlayerPrefs.SetString("winner", "Tie");
                SceneManager.LoadScene("BowlingWinnerScene");
                Debug.Log("Tie !!!");
            }
        }
    }

}

