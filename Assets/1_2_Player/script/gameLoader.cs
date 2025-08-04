using UnityEngine;
using UnityEngine.SceneManagement;

public class gameLoader : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void backBtn()
    {
        SceneManager.LoadScene("Main");
    }
}
