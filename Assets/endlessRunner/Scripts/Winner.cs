using UnityEngine;
using TMPro;

public class Winner : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public Camera mainCamera;

    void Start()
    {
        // Get winner name
        string winner = PlayerPrefs.GetString("WinnerName", "Unknown");

        // Update text
        winnerText.text = winner + " Wins!";

        // Change background color based on winner
        if (winner.Contains("Red"))
        {
            Camera.main.backgroundColor = Color.red;
            winnerText.color = Color.white;
        }
        else if (winner.Contains("Blue"))
        {
            Camera.main.backgroundColor = Color.blue;
            winnerText.color = Color.white;
        }
        else
        {
            Camera.main.backgroundColor = Color.black;
            winnerText.color = Color.yellow;
        }
    }
}
