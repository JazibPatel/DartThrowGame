using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class sceneManager : MonoBehaviour
{

    void Start()
    {

        //SceneLoader.instance.numOfPlayers;
        //SceneLoader.instance.difficulty;
    }

    public void loadDartThrow()
    {
        
            SceneManager.LoadScene("dartThrowScene");
       
    }
    public void backBtn()
    {
        SceneManager.LoadScene("Main"); 
    }
}
