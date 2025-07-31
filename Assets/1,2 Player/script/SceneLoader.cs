using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    public int numOfPlayers;
    public string difficulty;

    public void LoadPlayer1Scene()
    {
        SceneManager.LoadScene("Player_1");
    }

    public void setPlayer(int x)
    {
        numOfPlayers = x;
    }

    public void setDefficulty(string y)
    {
        difficulty = y;
    }

    private void Start()
    {
        instance = this;
    }
}
