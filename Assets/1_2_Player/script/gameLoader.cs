using UnityEngine;
using UnityEngine.SceneManagement;

public class gameLoader : MonoBehaviour
{
    public string sceneToLoad;  // Assign in Inspector

    // Load the selected game
    public void LoadGame()
    {
        // Use SceneLoader if it exists
        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.LoadGame(sceneToLoad);
        }
        else
        {
            // Fallback (shouldn't happen unless SceneLoader missing)
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Back to GameList (NOT Main)
    public void BackToGameList()
    {
        SceneManager.LoadScene("gameList");
    }

    // Back to Main (destroy SceneLoader)
    public void BackToMain()
    {
        if (SceneLoader.instance != null)
            SceneLoader.instance.BackToMain();
        else
            SceneManager.LoadScene("Main");
    }
}
