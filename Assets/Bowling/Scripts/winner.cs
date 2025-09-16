using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class winner : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {

        string winner = PlayerPrefs.GetString("winner", "tie");

        if(winner == "Red")
        {
            winnerText.text = "Red Plyer Wins !!!";
            mainCamera.backgroundColor = Color.red;
        }
        else if(winner == "Blue")
        {
            winnerText.text = "Blue Player Wins !!!";
            mainCamera.backgroundColor = Color.blue;
        }
        else
        {
            winnerText.text = "It's Tie !!!";
            mainCamera.backgroundColor = Color.gray;
        }

    }
}
