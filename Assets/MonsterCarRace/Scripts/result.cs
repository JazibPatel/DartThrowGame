using UnityEngine;
using UnityEngine.SceneManagement;

public class result : MonoBehaviour
{
    public static result instance;

    public Redtruck redTruck;
    public Bluetruck blueTruck;

    private bool gameEnded = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckWinner()
    {
        if (gameEnded)
            return; // prevent double trigger

        gameEnded = true;

        int redScore = redTruck.Score;
        int blueScore = blueTruck.Score;

        if (redScore > blueScore)
        {
            PlayerPrefs.SetString("WinnerName", "Red");
        }
        else if (blueScore > redScore)
        {
            PlayerPrefs.SetString("WinnerName", "Blue");
        }
        else
        {
            PlayerPrefs.SetString("WinnerName", "Tie");
        }

        SceneManager.LoadScene("Winner");
    }
}
