using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DisplayScript : MonoBehaviour
{
    public Text playerText;
    public Text difficultyText;

    void Start()
    {
        playerText.text = "Player: " + SceneLoader.instance.numOfPlayers;
        difficultyText.text = "Difficulty: " + SceneLoader.instance.difficulty;

    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main"); 
    }
}
