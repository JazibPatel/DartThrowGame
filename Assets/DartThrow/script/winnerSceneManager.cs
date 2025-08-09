using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winnerSceneManager : MonoBehaviour
{
    public Camera cam;
    public TMP_Text winnerText;

    void Start()
    {
        if (scoreManager.winnerPlayer == 1)
        {
            cam.backgroundColor = Color.red;
            winnerText.text = "RED \n WINNER!";
            winnerText.color = Color.white;
        }
        else if (scoreManager.winnerPlayer == 2)
        {
            cam.backgroundColor = Color.blue;
            winnerText.text = "BLUE \n WINNER!";
            winnerText.color = Color.white;
        }
        else
        {
            cam.backgroundColor = Color.gray;
            winnerText.text = "IT'S A TIE!";
            winnerText.color = Color.black;
        }
    }

    public void gameList()
    {
        SceneManager.LoadScene("gameList");
    }
}

